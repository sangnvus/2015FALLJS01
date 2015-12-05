"use strict";

app.controller('BackedHistoryProjectController', function ($scope, projects, UserService, ProjectService, DTOptionsBuilder, DTColumnDefBuilder) {
    $scope.ListBackedProjectHistory = projects.data.Data;
    getBackedUserInfo();

    // Define table
    $scope.dtOptions = DTOptionsBuilder.newOptions()
        .withDisplayLength(10)
        .withBootstrap()
        .withOption('order', [3, 'desc']);

    // Define column 
    $scope.dtColumnDefs = [
        DTColumnDefBuilder.newColumnDef(5).notSortable()
    ];


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

    $scope.getBackingInfo =  function(projectCode) {
        var promiseGet = ProjectService.backingInfo(projectCode);
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


