﻿///<reference path="jquery.ts" />


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

    private static regex: RegExp = /^[#?]?\(\s*([\+\-]?[0-9]+)\s*,\s*([\+\-]?[0-9])\s*\)$/;

    public static parse(str: string)
    {

      if (this.regex.test(str) == false)
      {
        window.alert("coordinate error");
      }


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

    static Run(fun: (coordinate: Coordinate) => void)
    {


      if (window.location.search == "")
        window.location.search = "(0,0)";

      var coordinate = Coordinate.parse(decodeURIComponent(window.location.search));

      fun(coordinate);



    }

    static Action(id: string)
    {

      var coordinate = Coordinate.parse(decodeURIComponent(window.location.search));

      $.getJSON("/" + coordinate.toString() + "/" + id, data => window.location.reload());

    }



    static Place(coordinate: Coordinate, fun: (place: Place) => void)
    {
      $.getJSON("/" + coordinate.toString(), data => fun(new Place(data)));
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


  export function bindPlace(place: Place, dom: JQuery)
  {
    var name = place.building.name;
    if (place.acting != null)
      name += "(" + place.acting.ActionDescriptor.Name + ")"
    dom.find("a").attr("href", "?" + place.coordinate.toString()).text(name);
  }

};






$(() =>
{

  hello.Client.Run(function (coordinate)
  {

    hello.Client.Place(coordinate, place =>
    {


      $("#map #placeO").text(place.building.name);


      hello.Client.Place(coordinate.NW, place => hello.bindPlace(place, $("#map #placeNW")));
      hello.Client.Place(coordinate.NE, place => hello.bindPlace(place, $("#map #placeNE")));
      hello.Client.Place(coordinate.E, place => hello.bindPlace(place, $("#map #placeE")));
      hello.Client.Place(coordinate.SE, place => hello.bindPlace(place, $("#map #placeSE")));
      hello.Client.Place(coordinate.SW, place => hello.bindPlace(place, $("#map #placeSW")));
      hello.Client.Place(coordinate.W, place => hello.bindPlace(place, $("#map #placeW")));


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

      var list = $("#resources ul");


      for (var key in info.Resources)
      {

        console.info(key);
        list.append(
          $("<li/>")
            .append($("<div/>").addClass("name").text(key)).addBack()
            .append($("<div/>").addClass("quantity").text(info.Resources[key])).addBack()
          );
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

