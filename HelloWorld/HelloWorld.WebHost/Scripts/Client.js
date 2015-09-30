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
            if (this.regex.test(str) == false) {
                window.alert("coordinate error");
            }
            var result = str.match(this.regex);
            return new Coordinate(parseInt(result[1]), parseInt(result[2]));
        };
        Coordinate.prototype.add = function (coordinate) {
            return new Coordinate(this.x + coordinate.x, this.y + coordinate.y);
        };
        Object.defineProperty(Coordinate.prototype, "NW", {
            get: function () {
                return this.add(new Coordinate(-1, -2));
            },
            enumerable: true,
            configurable: true
        });
        Object.defineProperty(Coordinate.prototype, "NE", {
            get: function () {
                return this.add(new Coordinate(1, -2));
            },
            enumerable: true,
            configurable: true
        });
        Object.defineProperty(Coordinate.prototype, "E", {
            get: function () {
                return this.add(new Coordinate(2, 0));
            },
            enumerable: true,
            configurable: true
        });
        Object.defineProperty(Coordinate.prototype, "SE", {
            get: function () {
                return this.add(new Coordinate(1, 2));
            },
            enumerable: true,
            configurable: true
        });
        Object.defineProperty(Coordinate.prototype, "SW", {
            get: function () {
                return this.add(new Coordinate(-1, 2));
            },
            enumerable: true,
            configurable: true
        });
        Object.defineProperty(Coordinate.prototype, "W", {
            get: function () {
                return this.add(new Coordinate(-2, 0));
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
            this.coordinate = Coordinate.parse(data.Coordinate);
            this.building = new Building(data.Building);
            this.actions = data.Actions;
            this.acting = data.Action;
        }
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
            fun(coordinate);
        };
        Client.Action = function (id) {
            var coordinate = Coordinate.parse(decodeURIComponent(window.location.search));
            $.getJSON("/" + coordinate.toString() + "/" + id, function (data) { return window.location.reload(); });
        };
        Client.Place = function (coordinate, fun) {
            $.getJSON("/" + coordinate.toString(), function (data) { return fun(new Place(data)); });
        };
        Client.GameInfo = function (fun) {
            $.getJSON("/", function (data) { return fun(data); });
        };
        Client.Messages = function (fun) {
            $.getJSON("/Messages", function (data) { return fun(data); });
        };
        return Client;
    })();
    hello.Client = Client;
    function bindPlace(place, dom) {
        var name = place.building.name;
        if (place.acting != null)
            name += "(" + place.acting.ActionDescriptor.Name + ")";
        dom.find("a").attr("href", "?" + place.coordinate.toString()).text(name);
    }
    hello.bindPlace = bindPlace;
})(hello || (hello = {}));
;
$(function () {
    hello.Client.Run(function (coordinate) {
        hello.Client.Place(coordinate, function (place) {
            $("#map #placeO").text(place.building.name);
            hello.Client.Place(coordinate.NW, function (place) { return hello.bindPlace(place, $("#map #placeNW")); });
            hello.Client.Place(coordinate.NE, function (place) { return hello.bindPlace(place, $("#map #placeNE")); });
            hello.Client.Place(coordinate.E, function (place) { return hello.bindPlace(place, $("#map #placeE")); });
            hello.Client.Place(coordinate.SE, function (place) { return hello.bindPlace(place, $("#map #placeSE")); });
            hello.Client.Place(coordinate.SW, function (place) { return hello.bindPlace(place, $("#map #placeSW")); });
            hello.Client.Place(coordinate.W, function (place) { return hello.bindPlace(place, $("#map #placeW")); });
            $("#building .name").text(place.building.name);
            $("#building .description").text(place.building.description);
            if (place.acting != null) {
                console.info(place.acting);
                $("#acting .name").text(place.acting.ActionDescriptor.Name);
                $("#acting .description").text(place.acting.ActionDescriptor.Description);
                $("#acting .remaining").text(place.acting.Remaining);
                $("#actions").remove();
            }
            else {
                $("#acting").remove();
                var list = $("#actions ul");
                place.actions.forEach(function (item) { return list.append($("<li/>").text(item.Name).click(function () { return hello.Client.Action(item.Guid); })); });
            }
        });
        hello.Client.GameInfo(function (info) {
            var list = $("#resources ul");
            for (var key in info.Resources) {
                console.info(key);
                list.append($("<li/>")
                    .append($("<div/>").addClass("name").text(key)).addBack()
                    .append($("<div/>").addClass("quantity").text(info.Resources[key])).addBack());
            }
        });
        hello.Client.Messages(function (messages) {
            var list = $("#messages ul");
            messages.forEach(function (item) {
                var date = new Date(item.NotifyTime);
                list.append($("<li/>")
                    .append($("<div/>").addClass("date").text(date.toLocaleString())).addBack()
                    .append($("<div/>").addClass("content").text(item.Content)).addBack());
            });
        });
    });
});
