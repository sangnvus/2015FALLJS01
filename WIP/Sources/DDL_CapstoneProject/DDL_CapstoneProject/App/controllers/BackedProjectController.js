"use strict";

app.controller('BackedProjectController', function ($scope, listsBacked) {
    $scope.ListBackedProject = listsBacked.data.Data;
    $scope.checkEmpty = $scope.ListBackedProject.length;


});
