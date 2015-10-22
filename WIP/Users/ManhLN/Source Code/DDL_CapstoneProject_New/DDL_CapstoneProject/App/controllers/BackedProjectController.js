"use strict";

app.controller('BackedProjectController', function ($scope, listsBacked) {
    $scope.ListBackedProject = listsBacked.data.Data;

    //function getBackedProject() {
    //    var promiseGet = ProjectService.getBackedProject();

    //        promiseGet.then(
    //            function (result) {
    //                if (result.data.Status === "success") {
    //                    $scope.ListBackedProject = result.data.Data;
    //                } else {
    //                    CommmonService.checkError(result.data.Type);
    //                    $scope.Error = result.data.Message;
    //                }
    //            },
    //            function (error) {
    //                $scope.Error = error.data.Message;
    //            });
    //    }
});
