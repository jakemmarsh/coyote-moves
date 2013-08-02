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

myModule.factory('requestForm', function ($http, $q) {
    return {

        apiPath: 'coyotemoves/api/',

        getAllJobTitles: function () {
            var deferred = $q.defer();

            $http.get(this.apiPath + 'Requestform/GetAllJobTitles/').success(function (data) {
                deferred.resolve(data);
            }).error(function () {
                deferred.reject("An error occurred while fetching job titles for autocomplete.");
            });

            return deferred.promise;
        },

        getAllDepartments: function () {
            var deferred = $q.defer();

            $http.get(this.apiPath + 'Requestform/GetAllDepartments/').success(function (data) {
                deferred.resolve(data);
            }).error(function () {
                deferred.reject("An error occurred while fetching job titles for autocomplete.");
            });

            return deferred.promise;
        },

        getAllGroups: function () {
            var deferred = $q.defer();

            $http.get(this.apiPath + 'Requestform/GetAllGroups/').success(function (data) {
                deferred.resolve(data);
            }).error(function () {
                deferred.reject("An error occurred while fetching job titles for autocomplete.");
            });

            return deferred.promise;
        }
    }
});