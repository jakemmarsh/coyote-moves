myModule.factory('desks', function ($http, $q) {
    return {

        apiPath: 'coyotemoves/api/',

        getDesksByFloor: function (floorId) {
            var deferred = $q.defer();

            $http.get(this.apiPath + 'Desk/GetDesksByFloor/' + floorId).success(function (data) {
                deferred.resolve(data);
            }).error(function () {
                deferred.reject("An error occurred while fetching desks by floor.");
            });

            return deferred.promise;
        }
    }
});