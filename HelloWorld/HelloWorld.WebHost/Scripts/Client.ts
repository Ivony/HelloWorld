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

    private static regex: RegExp = /^[#?]?\(\s*([\+\-]?[0-9]+)\s*,\s*([\+\-]?[0-9])\s*\)$/;

    public static parse(str: string)
    {

      console.info(str);
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



    public get A(): Coordinate
    {
      return this.add(new Coordinate(-1, -2));
    }


    public get B(): Coordinate
    {
      return this.add(new Coordinate(1, -2));
    }


    public get C(): Coordinate
    {
      return this.add(new Coordinate(-2, 0));
    }


    public get D(): Coordinate
    {
      return this.add(new Coordinate(2, 0));
    }


    public get E(): Coordinate
    {
      return this.add(new Coordinate(-1, 2));
    }


    public get F(): Coordinate
    {
      return this.add(new Coordinate(1, 2));
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


    private actions: Array<any>;
    private acting: any;


    constructor(data: any)
    {


      console.info(data);

      this.coordinate = Coordinate.parse(data.Coordinate);
      this.building = new Building(data.Building);
      this.actions = data.Actions;
      this.acting = data.Action;

    }


    public toHtml()
    {
      var html = "<section id='coordinate'><h3>Coordinate:</h3> " + this.coordinate + "</section>" +
        "<section id='building'><h3>Building:</h3> " + this.building.toHtml() + "</section>";


      if (this.actions != null)
      {

        html += "<section id='actions'><h3>Actions:</h3> ";
        this.actions.forEach(action => html += "<p onclick='hello.Client.Action( \"" + action.Guid + "\" );' style='cursor: pointer;'><b>" + action.Name + "</b> " + action.Description + "</p>");
        html += "</section>";
      }

      else if (this.acting != null)
      {
        html += "<section id='acting'><h3>Acting:</h3> ";
        html += "<p><b> " + this.acting.ActionDescriptor.Name + "</b> " + this.acting.ActionDescriptor.Description + "</p>";
        html += "<p>" + this.acting.Remaining + "</p>";
        html += "</section>";
      }

      return html;


    }

  }

  export class Client
  {

    static Run(fun: (place: Place) => void)
    {


      if (window.location.search == "")
        window.location.search = "(0,0)";

      var coordinate = Coordinate.parse(decodeURIComponent(window.location.search));

      $.getJSON("/" + coordinate.toString(), data => fun(new Place(data)));



    }

    static Action(id: string)
    {

      var coordinate = Coordinate.parse(decodeURIComponent(window.location.search));

      $.getJSON("/" + coordinate.toString() + "/" + id, data => window.location.reload());

    }

  }

};



$(() =>
{

  hello.Client.Run(function (place)
  {

    $("#place").html(place.toHtml());

  });

});

