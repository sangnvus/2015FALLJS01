"use strict";

app.controller('ShowBackedProjects', function ($scope, $filter, ProjectService) {

    // Get all current user's projects
    function loadProjectRecord() {
        var promiseGetProject = ProjectService.getBackedProjects();
        promiseGetProject.then(
            function (result) {
                $scope.Project = result.data;
            },
            function (error) {
                $scope.error = error;
            });
    }
    loadProjectRecord();
});