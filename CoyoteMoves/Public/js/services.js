myModule.factory('desks', function ($http, $q) {
    return {

        apiPath: '/api/',

        getDesksByFloor: function (floorId) {
            var deferred = $q.defer();

            $http.get(this.apiPath + 'Desks/GetByFloor/' + floorId).success(function (data) {
                deferred.resolve();
            }).error(function () {
                deferred.reject("An error occurred while fetching desks by floor.");
            });

            return deferred.promise;
        }
    }
});