///<reference path="jquery.ts" />
var hello;
(function (hello) {
    var Coordinate = (function () {
        function Coordinate(x, y) {
            this.x = x;
            this.y = y;
        }
        Coordinate.prototype.toString = function () {
            return "(" + this.x + ", " + this.y + ")";
        };
        Coordinate.parse = function (str) {
            console.info(str);
            if (this.regex.test(str) == false) {
                window.alert("coordinate error");
            }
            var result = str.match(this.regex);
            return new Coordinate(parseInt(result[1]), parseInt(result[2]));
        };
        Coordinate.prototype.add = function (coordinate) {
            return new Coordinate(this.x + coordinate.x, this.y + coordinate.y);
        };
        Object.defineProperty(Coordinate.prototype, "A", {
            get: function () {
                return this.add(new Coordinate(-1, -2));
            },
            enumerable: true,
            configurable: true
        });
        Object.defineProperty(Coordinate.prototype, "B", {
            get: function () {
                return this.add(new Coordinate(1, -2));
            },
            enumerable: true,
            configurable: true
        });
        Object.defineProperty(Coordinate.prototype, "C", {
            get: function () {
                return this.add(new Coordinate(-2, 0));
            },
            enumerable: true,
            configurable: true
        });
        Object.defineProperty(Coordinate.prototype, "D", {
            get: function () {
                return this.add(new Coordinate(2, 0));
            },
            enumerable: true,
            configurable: true
        });
        Object.defineProperty(Coordinate.prototype, "E", {
            get: function () {
                return this.add(new Coordinate(-1, 2));
            },
            enumerable: true,
            configurable: true
        });
        Object.defineProperty(Coordinate.prototype, "F", {
            get: function () {
                return this.add(new Coordinate(1, 2));
            },
            enumerable: true,
            configurable: true
        });
        Coordinate.regex = /^[#?]?\(\s*([\+\-]?[0-9]+)\s*,\s*([\+\-]?[0-9])\s*\)$/;
        return Coordinate;
    })();
    hello.Coordinate = Coordinate;
    var Building = (function () {
        function Building(data) {
            this.name = data.Name;
            this.description = data.Description;
        }
        Building.prototype.toHtml = function () {
            return "<b> " + this.name + "</b> " + this.description;
        };
        return Building;
    })();
    hello.Building = Building;
    var Place = (function () {
        function Place(data) {
            console.info(data);
            this.coordinate = Coordinate.parse(data.Coordinate);
            this.building = new Building(data.Building);
            this.actions = data.Actions;
            this.acting = data.Action;
        }
        Place.prototype.toHtml = function () {
            var html = "<section id='coordinate'><h3>Coordinate:</h3> " + this.coordinate + "</section>" +
                "<section id='building'><h3>Building:</h3> " + this.building.toHtml() + "</section>";
            if (this.actions != null) {
                html += "<section id='actions'><h3>Actions:</h3> ";
                this.actions.forEach(function (action) { return html += "<p onclick='hello.Client.Action( \"" + action.Guid + "\" );' style='cursor: pointer;'><b>" + action.Name + "</b> " + action.Description + "</p>"; });
                html += "</section>";
            }
            else if (this.acting != null) {
                html += "<section id='acting'><h3>Acting:</h3> ";
                html += "<p><b> " + this.acting.ActionDescriptor.Name + "</b> " + this.acting.ActionDescriptor.Description + "</p>";
                html += "<p>" + this.acting.Remaining + "</p>";
                html += "</section>";
            }
            return html;
        };
        return Place;
    })();
    hello.Place = Place;
    var Client = (function () {
        function Client() {
        }
        Client.Run = function (fun) {
            if (window.location.search == "")
                window.location.search = "(0,0)";
            var coordinate = Coordinate.parse(decodeURIComponent(window.location.search));
            $.getJSON("/" + coordinate.toString(), function (data) { return fun(new Place(data)); });
        };
        Client.Action = function (id) {
            var coordinate = Coordinate.parse(decodeURIComponent(window.location.search));
            $.getJSON("/" + coordinate.toString() + "/" + id, function (data) { return window.location.reload(); });
        };
        return Client;
    })();
    hello.Client = Client;
})(hello || (hello = {}));
;
$(function () {
    hello.Client.Run(function (place) {
        $("#place").html(place.toHtml());
    });
});
//# sourceMappingURL=Client.js.map