myModule.factory('user', function ($http, $q) {
    return {

        apiPath: 'coyotemoves/api/',

        getUserName: function () {
            var deferred = $q.defer();

            $http.get(this.apiPath + 'User/GetUserName/').success(function (data) {
                deferred.resolve(data);
            }).error(function () {
                deferred.reject("An error occurred while fetching user.");
            });

            return deferred.promise;
        },

        getUserAuthType: function () {
            var deferred = $q.defer();

            $http.get(this.apiPath + 'User/GetUserAuthType/').success(function (data) {
                deferred.resolve(data);
            }).error(function () {
                deferred.reject("An error occurred while fetching user.");
            });

            return deferred.promise;
        },

        getUserRoles: function () {
            var deferred = $q.defer();

            $http.get(this.apiPath + 'User/GetUserRoles/').success(function (data) {
                deferred.resolve(data);
            }).error(function () {
                deferred.reject("An error occurred while fetching user.");
            });

            return deferred.promise;
        },
    }
});

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

myModule.factory('autocomplete', function ($http, $q) {
    return {

        apiPath: 'coyotemoves/api/',

        getJobTitles: function (floorId) {
            var deferred = $q.defer();

            $http.get(this.apiPath + 'Desk/GetDesksByFloor/' + floorId).success(function (data) {
                deferred.resolve(data);
            }).error(function () {
                deferred.reject("An error occurred while fetching job titles for autocomplete.");
            });

            return deferred.promise;
        }
    }
});