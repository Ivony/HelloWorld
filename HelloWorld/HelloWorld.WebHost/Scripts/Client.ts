///<reference path="jquery.ts" />


module hello
{




  export class Coordinate
  {


    constructor(x: number, y: number)
    {
      this.x = x;
      this.y = y;
    }

    public x: number;
    public y: number;

    public toString(): string
    {
      return "(" + this.x + ", " + this.y + ")";
    }

    private static regex: RegExp = /^[#?]?\(\s*([\+\-]?[0-9]+)\s*,\s*([\+\-]?[0-9]+)\s*\)$/;

    public static parse(str: string)
    {

      if (this.regex.test(str) == false)
        throw "coordinate error";


      var result = str.match(this.regex);

      return new Coordinate(parseInt(result[1]), parseInt(result[2]));

    }



    public add(coordinate: Coordinate)
    {
      return new Coordinate(this.x + coordinate.x, this.y + coordinate.y);
    }



    public get NW(): Coordinate
    {
      return this.add(new Coordinate(-1, -2));
    }


    public get NE(): Coordinate
    {
      return this.add(new Coordinate(1, -2));
    }


    public get E(): Coordinate
    {
      return this.add(new Coordinate(2, 0));
    }


    public get SE(): Coordinate
    {
      return this.add(new Coordinate(1, 2));
    }


    public get SW(): Coordinate
    {
      return this.add(new Coordinate(-1, 2));
    }


    public get W(): Coordinate
    {
      return this.add(new Coordinate(-2, 0));
    }


    public static get Root(): Coordinate
    {
      return new Coordinate(0, 0);
    }

  }


  export class Unit
  {

    public ID: string;
    public Name: string;
    public Coordinate: Coordinate;

    constructor(data: any)
    {
      this.ID = data.Guid;
      this.Name = data.Name;
      this.Coordinate = Coordinate.parse(data.Coordinate);
    }

  }


  export class Building
  {
    public name: string;
    public description: string;

    constructor(data: any)
    {
      this.name = data.Name;
      this.description = data.Description
    }


    public toHtml()
    {
      return "<b> " + this.name + "</b> " + this.description;

    }

  }

  export class Place
  {

    public coordinate: Coordinate;
    public building: Building;


    public actions: Array<any>;
    public acting: any;


    constructor(data: any)
    {


      this.coordinate = Coordinate.parse(data.Coordinate);
      this.building = new Building(data.Building);
      this.actions = data.Actions;
      this.acting = data.Action;

    }

  }

  export class Client
  {

    static Run(fun: (unit: Unit) => void)
    {


      $.ajax("/", {
        error: xhr =>
        {
          if (xhr.status == 401)
          {
            window.location.href = "/Login.html";
          }
        }
      });


      var unitId = decodeURIComponent(window.location.search).substring(1);

      if (unitId == null || unitId == "")
        fun(null);

      else
        hello.Client.Unit(unitId, unit => fun(unit));
    }

    static Action(id: string)
    {

      $.getJSON("/" + coordinate.toString() + "/" + id, data => window.location.reload());

    }



    static Place(coordinate: Coordinate, fun: (place: Place) => void)
    {
      $.getJSON("/" + coordinate.toString(), data => fun(new Place(data)));
    }


    static Unit(unitId: string, fun: (unit: Unit) => void)
    {
      $.getJSON("/Unit/" + unitId, data => fun(new Unit(data)));
    }



    static GameInfo(fun: (info: any) => void)
    {
      $.getJSON("/", data => fun(data));
    }



    static Messages(fun: (messages: any[]) => void)
    {
      $.getJSON("/Messages", data => fun(data));
    }


  }


  export function bindPlace(place: Place, direction: string, unit: Unit, dom: JQuery)
  {
    var name = place.building.name;
    if (place.acting != null)
      name += "(" + place.acting.ActionDescriptor.Name + ")"

    if (unit != null)
      dom.find("a").text(name).css("cursor", "pointer").click(() => moveTo(unit.ID, direction));
    else
      dom.find("a").text(name);
  }

  export function moveTo(unitId: string, direction: string)
  {
    console.info(unitId + " move to " + direction);

    $.get("/Unit/" + unitId + "/Move/" + direction, function ()
    {
      window.location.reload();
    })
  }

};




declare var coordinate: hello.Coordinate;

$(() =>
{

  hello.Client.Run(function (unit)
  {

    if (unit == null)
      coordinate = hello.Coordinate.Root;

    else
      coordinate = unit.Coordinate;

    hello.Client.Place(coordinate, place =>
    {

      $("#map #placeO").text(place.building.name).append($("<div />").text(coordinate.toString()));


      hello.Client.Place(coordinate.NW, place => hello.bindPlace(place, "NW", unit, $("#map #placeNW")));
      hello.Client.Place(coordinate.NE, place => hello.bindPlace(place, "NE", unit, $("#map #placeNE")));
      hello.Client.Place(coordinate.E, place => hello.bindPlace(place, "E", unit, $("#map #placeE")));
      hello.Client.Place(coordinate.SE, place => hello.bindPlace(place, "SE", unit, $("#map #placeSE")));
      hello.Client.Place(coordinate.SW, place => hello.bindPlace(place, "SW", unit, $("#map #placeSW")));
      hello.Client.Place(coordinate.W, place => hello.bindPlace(place, "W", unit, $("#map #placeW")));


      $("#building .name").text(place.building.name);
      $("#building .description").text(place.building.description);


      if (place.acting != null)
      {
        console.info(place.acting);

        $("#acting .name").text(place.acting.ActionDescriptor.Name);
        $("#acting .description").text(place.acting.ActionDescriptor.Description);
        $("#acting .remaining").text(place.acting.Remaining);
        $("#actions").remove();
      }
      else
      {
        $("#acting").remove();
        var list = $("#actions ul");

        place.actions.forEach(item => list.append($("<li/>").text(item.Name).click(() => hello.Client.Action(item.Guid))));

      }
    });




    hello.Client.GameInfo(info =>
    {

      {
        let list = $("#resources ul");


        for (var key in info.Resources)
        {

          list.append(
            $("<li/>")
              .append($("<div/>").addClass("name").text(key)).addBack()
              .append($("<div/>").addClass("quantity").text(info.Resources[key])).addBack()
          );
        }
      }


      {
        var list = $("#units ul");

        for (var i = 0; i < info.Units.length; i++)
        {

          var item = info.Units[i];
          console.info(item);
          list.append($("<li/>")
            .append($("<a/>").addClass("name").text(item.Name).attr("href", "?" + item.Guid)).addBack()
            .append("/").addBack()
            .append($("<span/>").text(item.Mobility)).addBack()
          );

        }
      }
    });



    hello.Client.Messages(messages =>
    {

      var list = $("#messages ul");

      messages.forEach(item =>
      {
        var date = new Date(item.NotifyTime);



        list.append(
          $("<li/>")
            .append($("<div/>").addClass("date").text(date.toLocaleString())).addBack()
            .append($("<div/>").addClass("content").text(item.Content)).addBack()
        );
      });

    });


  });

});

