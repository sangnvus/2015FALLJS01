"use strict";

app.controller('ShowProjectsController', function ($scope, $location, ProjectService) {
    function loadAllProjectRecords() {
        var promiseGetProject = ProjectService.getProjects();
        promiseGetProject.then(
            function(result) {
                $scope.Projects = result.data;
            },
            function(error) {
                $scope.error = error;
            });
    }

    loadAllProjectRecords();

    //To Edit Project  
    $scope.editProject = function (id) {
        $location.path("/editProject/" + id);
    }

    //To Delete a Project  
    $scope.deleteProject = function (id) {
        $location.path("/deleteProject/" + id);
    }
});