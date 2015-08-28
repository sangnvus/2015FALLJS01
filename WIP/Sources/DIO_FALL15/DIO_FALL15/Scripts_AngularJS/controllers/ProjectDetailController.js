"use strict";

app.controller('ProjectDetailController', function ($scope, $location, $routeParams, ProjectService) {

    var Id = $routeParams.id;

    function loadProjectRecord() {
        var promiseGetProject = ProjectService.getProject(Id);
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