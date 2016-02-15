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
            if (this.regex.test(str) == false)
                throw "coordinate error";
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
        Object.defineProperty(Coordinate, "Root", {
            get: function () {
                return new Coordinate(0, 0);
            },
            enumerable: true,
            configurable: true
        });
        Coordinate.regex = /^[#?]?\(\s*([\+\-]?[0-9]+)\s*,\s*([\+\-]?[0-9]+)\s*\)$/;
        return Coordinate;
    })();
    hello.Coordinate = Coordinate;
    var Unit = (function () {
        function Unit(data) {
            this.ID = data.Guid;
            this.Name = data.Name;
            this.Coordinate = Coordinate.parse(data.Coordinate);
        }
        return Unit;
    })();
    hello.Unit = Unit;
    var Building = (function () {
        function Building(data) {
            this.name = data.Name;
            this.description = data.Description;
        }
        return Building;
    })();
    hello.Building = Building;
    var Terrain = (function () {
        function Terrain(data) {
            this.name = data.Name;
            this.description = data.Description;
        }
        return Terrain;
    })();
    hello.Terrain = Terrain;
    var Place = (function () {
        function Place(data) {
            this.coordinate = Coordinate.parse(data.Coordinate);
            this.building = data.Building == null ? null : new Building(data.Building);
            this.terrain = data.Terrain == null ? null : new Terrain(data.Terrain);
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
            $.ajax("/", {
                error: function (xhr) {
                    if (xhr.status == 401) {
                        window.location.href = "/Login.html";
                    }
                }
            });
            var unitId = decodeURIComponent(window.location.search).substring(1);
            if (unitId == null || unitId == "")
                fun(null);
            else
                hello.Client.Unit(unitId, function (unit) { return fun(unit); });
        };
        Client.Action = function (id) {
            $.getJSON("/" + coordinate.toString() + "/" + id, function (data) { return window.location.reload(); });
        };
        Client.Place = function (coordinate, fun) {
            $.getJSON("/" + coordinate.toString(), function (data) { return fun(new Place(data)); });
        };
        Client.Unit = function (unitId, fun) {
            $.getJSON("/Unit/" + unitId, function (data) { return fun(new Unit(data)); });
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
    function bindPlace(place, direction, unit, dom) {
        if (place.building != null)
            dom.find(".building").text(place.building.name);
        if (place.terrain != null)
            dom.find(".terrain").text(place.terrain.name);
        dom.find(".coordinate").text(place.coordinate.toString());
        if (unit != null)
            dom.find("a").css("cursor", "pointer").click(function () { return moveTo(unit.ID, direction); });
    }
    hello.bindPlace = bindPlace;
    function moveTo(unitId, direction) {
        console.info(unitId + " move to " + direction);
        $.get("/Unit/" + unitId + "/Move/" + direction, function () {
            window.location.reload();
        });
    }
    hello.moveTo = moveTo;
})(hello || (hello = {}));
;
$(function () {
    hello.Client.Run(function (unit) {
        if (unit == null)
            coordinate = hello.Coordinate.Root;
        else
            coordinate = unit.Coordinate;
        hello.Client.Place(coordinate, function (place) {
            hello.bindPlace(place, null, null, $("#map #placeO"));
            hello.Client.Place(coordinate.NW, function (place) { return hello.bindPlace(place, "NW", unit, $("#map #placeNW")); });
            hello.Client.Place(coordinate.NE, function (place) { return hello.bindPlace(place, "NE", unit, $("#map #placeNE")); });
            hello.Client.Place(coordinate.E, function (place) { return hello.bindPlace(place, "E", unit, $("#map #placeE")); });
            hello.Client.Place(coordinate.SE, function (place) { return hello.bindPlace(place, "SE", unit, $("#map #placeSE")); });
            hello.Client.Place(coordinate.SW, function (place) { return hello.bindPlace(place, "SW", unit, $("#map #placeSW")); });
            hello.Client.Place(coordinate.W, function (place) { return hello.bindPlace(place, "W", unit, $("#map #placeW")); });
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
            {
                var list_1 = $("#resources ul");
                for (var key in info.Resources) {
                    list_1.append($("<li/>")
                        .append($("<div/>").addClass("name").text(key)).addBack()
                        .append($("<div/>").addClass("quantity").text(info.Resources[key])).addBack());
                }
            }
            {
                var list = $("#units ul");
                for (var i = 0; i < info.Units.length; i++) {
                    var item = info.Units[i];
                    console.info(item);
                    list.append($("<li/>")
                        .append($("<a/>").addClass("name").text(item.Name).attr("href", "?" + item.Guid)).addBack()
                        .append("/").addBack()
                        .append($("<span/>").text(item.Mobility)).addBack());
                }
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
//# sourceMappingURL=Client.js.map