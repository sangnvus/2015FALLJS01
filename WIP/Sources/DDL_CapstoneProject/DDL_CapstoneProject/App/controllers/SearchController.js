"use strict";

var searchBlockSize = 12;
app.controller('SearchController',
    ['$scope', '$http', 'projectbycategory', 'categoryList', 'ProjectService', 'CategoryService', 'isAdvance', 'selectedorder', 'selectcategory', 'searchkey', 'projectResultListSize',
function ($scope, $http, projectbycategory, categoryList, ProjectService, CategoryService, isAdvance, selectedorder, selectcategory, searchkey, projectResultListSize) {
    //Todo here.
    $scope.projectbycategory = projectbycategory.data.Data;
    $scope.categoryList = [{ "CategoryID": "all", "Name": "Tất cả" }].concat(categoryList.data.Data)
    $scope.projectResultListSize = projectResultListSize.data.Data;
    $scope.isAdvance = isAdvance;


    $scope.SortList = [{ 'type': 'PopularPoint', 'Label': 'Phổ biến' },
        { 'type': 'CreatedDate', 'Label': 'Mới nhất' },
        { 'type': 'ExpireDate', 'Label': 'Ngày còn lại' },
        { 'type': 'CurrentFunded', 'Label': 'Ủng hộ' }];
    for (var i in $scope.SortList) {
        if (selectedorder == $scope.SortList[i].type)
             $scope.selectedorder = angular.copy($scope.SortList[i]);
    }

    $scope.searchkey = searchkey;
    for (var i = 0; i < $scope.categoryList.length; i++) {
        if ($scope.categoryList[i].CategoryID == selectcategory) {
            $scope.selectcategory = [$scope.categoryList[i]];
        }
    };
    $scope.statuss = [
        { 'isExpried': '', 'Label': 'Tất cả' },
        { 'isExpried': 'true', 'Label': 'Đã hết hạn' },
        { 'isExpried': 'false', 'Label': 'Đang gây vốn' }
    ];

    $scope.selectstatus = angular.copy($scope.statuss[0]);
    if ($scope.projectResultListSize == 0) {
        $scope.noti = "không tìm thấy dự án nào.";
    } else {
        $scope.noti = "Có " + $scope.projectResultListSize + "dự án";
    }


    $scope.index = searchBlockSize;
    $scope.Search = function (category, order, searchkey, selectstatus) {
        if (selectstatus.length == 0) {
            $scope.selectstatus = angular.copy($scope.statuss[0]);
        } else {
            for (var i in $scope.statuss) {
                if (selectstatus == $scope.statuss[i].isExpried) {
                    $scope.selectstatus = angular.copy($scope.statuss[i]);
                }
            }
        }
        var categoryIdString = "";
        var selectedCategory = angular.copy(category);
        if (!selectedCategory) {
            categoryIdString = "all";
            $scope.selectcategory = [{ "CategoryID": "all", "Name": "Tất cả" }];
        }
        else {
            if ((selectedCategory.length + 1) == $scope.categoryList.length || selectedCategory.length == 0) {
                categoryIdString = "all";
                $scope.selectcategory = [{ "CategoryID": "all", "Name": "Tất cả" }];
            } else {
                for (var i = 0; i < selectedCategory.length; i++) {
                    if (selectedCategory[i].CategoryID == "all") {
                        if (i == selectedCategory.length - 1) {
                            categoryIdString = "all";
                            $scope.selectcategory = [{ "CategoryID": "all", "Name": "Tất cả" }];
                            break;
                        } else {
                            $scope.selectcategory.splice(i, 1);
                            $scope.selectcategory = angular.copy($scope.selectcategory);
                        }
                    } else {
                        categoryIdString += "|" + selectedCategory[i].CategoryID + "|";
                    }
                }
            }
        }
        ProjectService.SearchProject(0, categoryIdString, order, searchkey, $scope.selectstatus.isExpried).then(function (projectlist) {
            $scope.projectbycategory = projectlist.data.Data;
            ProjectService.SearchCount(categoryIdString, searchkey, $scope.selectstatus.isExpried).then(function (response) {
                $scope.projectResultListSize = response.data.Data;
                if ($scope.projectResultListSize == 0) {
                    $scope.noti = "không tìm thấy dự án nào.";
                } else {
                    $scope.noti = "Có " + $scope.projectResultListSize + "dự án";
                }
                if ($scope.projectbycategory.length >= $scope.projectResultListSize) {
                    $scope.showLoadMoreButton = false;
                } else {
                    $scope.showLoadMoreButton = true;
                }
            })

            $scope.index = $scope.projectbycategory.length;
        });
    }
    
    if ($scope.projectbycategory.length >= $scope.projectResultListSize) {
        $scope.showLoadMoreButton = false;
    } else {
        $scope.showLoadMoreButton = true;
    }
    $scope.Loadmore = function (category, order, searchkey) {
        var categoryIdString = "";
        for (var i = 0; i < category.length; i++) {
            if (category[i].CategoryID == "all") {
                categoryIdString = "all";
                break;
            } else {
                categoryIdString += "|" + category[i].CategoryID + "|";
            }
        }
        ProjectService.SearchProject($scope.index, categoryIdString, order, searchkey, $scope.selectstatus.isExpried).then(function (projectlist) {
            $scope.projectbycategory = $scope.projectbycategory.concat(projectlist.data.Data);
            $scope.index = $scope.projectbycategory.length;
            if ($scope.projectbycategory.length >= $scope.projectResultListSize) {
                $scope.showLoadMoreButton = false;
            }
        });
    }

}]);