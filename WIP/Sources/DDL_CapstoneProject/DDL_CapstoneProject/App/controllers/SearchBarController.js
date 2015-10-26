"use strict";

app.controller('SearchBarController',
    function ($scope, $location) {
        $scope.submit = function (searchkey) {
            $location.path('/search').search({ advance: 'true', searchkey: searchkey });
        }
    });