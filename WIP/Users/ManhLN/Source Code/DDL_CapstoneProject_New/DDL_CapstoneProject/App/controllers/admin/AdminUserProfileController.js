"use strict";

app.controller('AdminUserProfileController',
    function ($scope, $sce, $rootScope, toastr, userprofile, AdminUserService, CommmonService,
        DTOptionsBuilder, DTColumnDefBuilder) {
        $scope.UserProfile = userprofile.data.Data;

        $scope.trustSrc = function (src) {
            return $sce.trustAsResourceUrl(src);
        }

        // Function check string startwith 'http'
        $scope.checkHTTP = function (input) {
            var lowerStr = (input + "").toLowerCase();
            return lowerStr.indexOf('http') == 0;
        }

        $scope.checkLoadBackedlist = false;
        $scope.getBackedProject = function () {
            if ($scope.checkLoadBackedlist == false) {
                var promise = AdminUserService.getBackedProject($scope.UserProfile.UserName);
                promise.then(
                    function (result) {
                        $scope.ListBacked = result.data.Data;
                        $scope.checkLoadBackedlist = true;
                    }
                 );
            }
        };

        $scope.checkLoadCreatedlist = false;
        $scope.getCreatedProject = function () {
            if ($scope.checkLoadCreatedlist == false) {
                var promise = AdminUserService.getCreatedProject($scope.UserProfile.UserName);
                promise.then(
                    function (result) {
                        $scope.ListCreated = result.data.Data;
                        $scope.checkLoadCreatedlist = true;
                    }
                 );
            }
        };

        $scope.getBackingDetail = function () {
            var promise = AdminUserService.getBackingDetail($scope.UserProfile.UserName);
            promise.then(
                function (result) {
                    $scope.BackingDetail = result.data.Data;
                }
             );
        };

    });