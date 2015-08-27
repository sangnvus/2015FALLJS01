"use strict";

app.controller('CurrentUserProjectsController', function ($scope, $location,  ProjectService) {

    // Get all current user's projects
    function loadProjectRecord() {
        var promiseGetProject = ProjectService.getYourProjects();
        promiseGetProject.then(
            function (result) {
                $scope.Project = result.data;
            },
            function (error) {
                $scope.error = error;
            });
    }
    loadProjectRecord();

    //To Edit Project  
    $scope.editProject = function (id) {
        $location.path("/editProject/" + id);
    }

    //To Delete a Project  
    $scope.deleteProject = function (id) {
        $location.path("/deleteProject/" + id);
    }
});