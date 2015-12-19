"use strict"

app.controller('ListBackerController', function ($scope, $route, projects, UserService, ProjectService, DTOptionsBuilder, DTColumnDefBuilder) {
    
    var promise = ProjectService.getProjectDetail($route.current.params.code);
    promise.then(

        function (result) {
            $scope.ListBacker = projects.data.Data.listBacker;
            $scope.labels = projects.data.Data.Date;
            $scope.series = ['Số tiền đã được ủng hộ', 'Mục tiêu'];
            var data2 = [];
            for (var i = 0; i < projects.data.Data.Amount.length; i++) {
                data2.push(result.data.Data.FundingGoal);
            }
            $scope.data = [projects.data.Data.Amount, data2];
            $scope.colors = ['#97bbcd', '#f7464a'];
            $scope.options = {
                multiTooltipTemplate: function (label) {
                    return (label.datasetLabel + ': ' + label.value.toString().replace(/\B(?=(\d{3})+(?!\d))/g, ",")) + "₫";
                },
                scaleLabel: function (label) {
                    return (label.value.toString().replace(/\B(?=(\d{3})+(?!\d))/g, ",")) + "₫";
                }
            };
        },
         function (error) {
             $scope.Error = error.data.Message;
         });
   
    //loadlistBacker();
        // Define table
    $scope.dtOptions = DTOptionsBuilder.newOptions()
    .withDisplayLength(10)
    .withOption('order', [3, 'desc'])
    .withBootstrap();

    $scope.dtColumnDefs = [
        DTColumnDefBuilder.newColumnDef(4).notSortable()
    ];

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
