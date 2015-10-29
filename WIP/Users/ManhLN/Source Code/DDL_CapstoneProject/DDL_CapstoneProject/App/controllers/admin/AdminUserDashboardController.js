"use strict";

app.controller('AdminUserDashboardController',
    function ($scope, $rootScope, $sce, toastr, userdashboard, AdminUserService, CommmonService,
        DTOptionsBuilder, DTColumnDefBuilder) {
        //Todo here.
        $scope.Dashboard = userdashboard.data.Data;
        $scope.trustSrc = function (src) {
            return $sce.trustAsResourceUrl(src);
        }

        // Function check string startwith 'http'
        $scope.checkHTTP = function (input) {
            var lowerStr = (input + "").toLowerCase();
            return lowerStr.indexOf('http') == 0;
        }
        $scope.NotIdleuser = $scope.Dashboard.Creator + $scope.Dashboard.Backer;
        $scope.labels = ["Người dùng đã có hoạt động", "Người dùng chưa có hoạt động","Người dùng chưa kích hoạt"];
        $scope.data = [$scope.NotIdleuser, $scope.Dashboard.IdleUser, $scope.Dashboard.NotVerifiedUser];
    });