"use strict";

app.controller('PublicProfileController', function ($scope, userpublicinfo) {
    $scope.UserBasicInfo = userpublicinfo.data.Data;

});