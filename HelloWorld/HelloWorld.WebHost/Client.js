var hello;
(function (hello) {
    var Coordinate = (function () {
        function Coordinate() {
        }
        return Coordinate;
    })();

    var Place = (function () {
        function Place(coordinate) {
            this.coordinate = coordinate;
        }
        return Place;
    })();

    var Client = (function () {
        function Client() {
        }
        Client.GetPlace = function (coordinate) {
            return new Place(coordinate);
        };
        return Client;
    })();
})(hello || (hello = {}));
;
