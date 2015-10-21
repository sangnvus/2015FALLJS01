"use strict";

app.controller('BackedHistoryProjectController', function ($scope, projects, UserService) {
    $scope.ListBackedProjectHistory = projects.data.Data;

    function getBackedProjectHistory() {
        var promiseGet = UserService.getUserBackedInfo();

            promiseGet.then(
                function (result) {
                    if (result.data.Status === "success") {
                        $scope.ListBackedUser = result.data.Data;
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
