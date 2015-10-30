"use strict";

app.controller('AdminBackingListController',
    function ($scope, $sce, $rootScope, toastr, listBacking, AdminUserService, CommmonService,
        DTOptionsBuilder, DTColumnDefBuilder) {
        //Todo here.
        $scope.ListBacking = listBacking.data.Data;

        $scope.trustSrc = function (src) {
            return $sce.trustAsResourceUrl(src);
        }

        // Function check string startwith 'http'
        $scope.checkHTTP = function (input) {
            var lowerStr = (input + "").toLowerCase();
            return lowerStr.indexOf('http') == 0;
        }
        // Define table
        $scope.dtOptions = DTOptionsBuilder.newOptions()
        .withDisplayLength(10)
        .withOption('order', [0, 'asc'])
        .withOption('stateSave', true)
        .withBootstrap();

        $scope.dtColumnDefs = [
            DTColumnDefBuilder.newColumnDef(5).notSortable()
        ];

        $scope.GetBacker = function (userName, backingID) {
            var promise = AdminUserService.getBackker(userName, backingID);
            promise.then(
                function (result) {
                    if (result.data.Status === "success") {
                        $scope.Backer = result.data.Data;
                    } else {
                        CommmonService.checkError(result.data.Type, $rootScope.BaseUrl);
                        $scope.Error = result.data.Message;
                    }
                },
                function (error) {
                    $scope.Error = error.data.Message;
                });
        }

    });