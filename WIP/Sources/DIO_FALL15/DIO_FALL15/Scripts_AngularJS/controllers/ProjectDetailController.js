"use strict";

app.controller('ProjectDetailController', function ($scope, $location, $routeParams,$cookies,$window, ProjectService) {
    var Id = $routeParams.id;
    $scope.ProjectExprire = false;
    function loadProjectRecord() {
        var promiseGetProject = ProjectService.getProject(Id);
        promiseGetProject.then(
            function (result) {
                $scope.Project = result.data;
                CheckExpire();
            },
            function (error) {
                $scope.error = error;
            });
    }

    loadProjectRecord();

    function CheckExpire() {
        var now = new Date();
        var deadline = new Date($scope.Project.Deadline);
        $scope.ProjectExprire = (deadline <= now);
    }

    $scope.OpenBackDialog = function() {
        var currentUser = $cookies.get("CurrentUser");
        if (currentUser == null) {
            $window.location.href = "http://localhost:14069/Account/Login";
        } else {
            $scope.BackAmount = null;
            $scope.BackForm.$setPristine();
            $('#exampleModal').modal('show');
        }
    }

    $scope.Back = function () {
        var amount = $scope.BackAmount;
        var promiseBack = ProjectService.BackProject(Id, amount);
        promiseBack.then(
            function (result) {
                $scope.Project.CurrentFund = result.data;           
                $('#exampleModal').modal('hide');
                $scope.BackAmount = null;
                alert("Back successfully! Thank you!");
            },
            function (error) {
                $scope.error = error;
            });
    }

    //To Edit Project  
    $scope.editProject = function (id) {
        $location.path("/editProject/" + id);
    }

    //To Delete a Project  
    $scope.deleteProject = function (id) {
        $location.path("/deleteProject/" + id);
    }
});