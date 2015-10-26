"use strict";

var ps;

app.controller('BackedHistoryProjectController', function ($scope, projects, UserService, ProjectService) {
    $scope.ListBackedProjectHistory = projects.data.Data;
    getBackedUserInfo();
    function getBackedUserInfo(){
        var promiseGet = UserService.getBackedUserInfo();
       
            promiseGet.then(
                function (result) {
                    if (result.data.Status === "success") {
                        $scope.BackedUser = result.data.Data;
                    } else {
                        CommmonService.checkError(result.data.Type);
                        $scope.Error = result.data.Message;
                    }
                },
                function (error) {
                    $scope.Error = error.data.Message;
                });
    }

    function getBackingInfo(projectId) {
        var promiseGet = ProjectService.backingInfo(projectId);
        promiseGet.then(
            function (result) {
                if (result.data.Status === "success") {
                    $scope.BackingInfo = result.data.Data;
                } else {
                    CommmonService.checkError(result.data.Type);
                    $scope.Error = result.data.Message;
                }
            },
            function (error) {
                $scope.Error = error.data.Message;
            });
    }
});


