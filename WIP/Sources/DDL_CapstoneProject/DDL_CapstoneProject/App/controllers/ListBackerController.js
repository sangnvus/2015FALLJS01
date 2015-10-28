"use strict"

app.controller('ListBackerController', function ($scope, $route, projects, UserService, ProjectService, DTOptionsBuilder, DTColumnDefBuilder) {
    //$scope.Project = project.data.Data;
    $scope.ListBacker = projects.data.Data.listBacker;
    $scope.labels = projects.data.Data.Date;
    $scope.series = ['Số tiền đã ủng hộ'];
    $scope.data = [projects.data.Data.Amount];
    //loadlistBacker();
        // Define table
    $scope.dtOptions = DTOptionsBuilder.newOptions()
    .withDisplayLength(10)
    //.withOption('order', [3, 'desc'])
    .withBootstrap();

    $scope.dtColumnDefs = [
        DTColumnDefBuilder.newColumnDef(0).notSortable()
    ];

    //function getBackedUserInfo(){
    //    var promiseGet = UserService.getBackedUserInfo();

    //        promiseGet.then(
    //            function (result) {
    //                if (result.data.Status === "success") {
    //                    $scope.BackedUser = result.data.Data;
    //                } else {
    //                    CommmonService.checkError(result.data.Type);
    //                    $scope.Error = result.data.Message;
    //                }
    //            },
    //            function (error) {
    //                $scope.Error = error.data.Message;
    //            });
    //}

    $scope.getBackingInfo = function () {
        var promiseGet = ProjectService.backingInfo($route.current.params.code);
        promiseGet.then(
            function (result) {
                if (result.data.Status === "success") {
                    $scope.BackingInfo = result.data.Data[0];
                    //console.log($scope.BackingInfo[0].BackingDiscription)
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
