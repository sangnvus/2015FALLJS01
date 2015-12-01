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
    $scope.selectedorder = selectedorder;
    $scope.searchkey = searchkey;
    for (var i = 0; i < $scope.categoryList.length; i++) {
        if ($scope.categoryList[i].CategoryID == selectcategory) {
            $scope.selectcategory = [$scope.categoryList[i]];
        }
    };
    $scope.statuss = [{ 'Value': 'true', 'Label': 'Đã hết hạn' },
        { 'Value': 'false', 'Label': 'Đang gây vốn' }];
    $scope.selectstatus = [$scope.statuss[1]];
    var statusString = "true";
    if (projectResultListSize.data.Data == 0) {
        document.getElementById("noResult").setAttribute("style", "display:inline;");
    } else {
        document.getElementById("noResult").setAttribute("style", "display:none;");
    }


    $scope.index = searchBlockSize;
    $scope.Search = function (category, order, searchkey, selectstatus) {
        if (selectstatus.length == 0) {
            $scope.selectstatus = [{ 'Value': 'false', 'Label': 'Đang gây vốn' }];
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

        statusString = "";
        for (var i = 0; i < selectstatus.length; i++) {
            statusString += "|" + selectstatus[i].Value + "|";
        }
        var searchResult = ProjectService.SearchProject(0, categoryIdString, order, searchkey, statusString).then(function (projectlist) {
            $scope.projectbycategory = projectlist.data.Data;
            ProjectService.SearchCount(categoryIdString, searchkey, statusString).then(function (response) {

                if (response.data.Data == 0) {

                    document.getElementById("noResult").setAttribute("style","display:inline;");
                } else {
                    document.getElementById("noResult").setAttribute("style", "display:none;");
                }
                $scope.projectResultListSize = response.data.Data;
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
        statusString = "";
        for (var i = 0; i < $scope.selectstatus.length; i++) {
            statusString += "|" + $scope.selectstatus[i].Value + "|";
        }
        var searchResult = ProjectService.SearchProject($scope.index, categoryIdString, order, searchkey, statusString).then(function (projectlist) {
            $scope.projectbycategory = $scope.projectbycategory.concat(projectlist.data.Data);
            $scope.index = $scope.projectbycategory.length;
            if ($scope.projectbycategory.length >= $scope.projectResultListSize) {
                $scope.showLoadMoreButton = false;
            }
        });
    }

    $scope.SortList = [{ 'Value': 'PopularPoint', 'Label': 'Phổ biến' },
        { 'Value': 'CreatedDate', 'Label': 'Mới nhất' },
        { 'Value': 'ExpireDate', 'Label': 'Ngày còn lại' },
        { 'Value': 'CurrentFunded', 'Label': 'Ủng hộ' }];
}]);