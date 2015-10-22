"use strict";

app.controller('StarredProjectController', function ($scope, project) {
    $scope.ListStarredProject = project.data.Data;
});