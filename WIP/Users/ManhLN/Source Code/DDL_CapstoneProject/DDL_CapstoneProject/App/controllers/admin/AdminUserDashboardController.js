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
        $scope.labels = ["Thành viên đã có hoạt động", "Thành viên chưa có hoạt động","Thành viên chưa kích hoạt"];
        $scope.data = [$scope.NotIdleuser, $scope.Dashboard.IdleUser, $scope.Dashboard.NotVerifiedUser];
        $scope.colors = ['#FDB45C', '#46BFBD', '#F7464A'];
    });