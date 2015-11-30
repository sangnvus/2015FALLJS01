"use strict";

app.controller('SearchBarController',
    function ($scope, $location, ProjectService) {
        function getProjectList() {
            return ProjectService.projectTitleList("").then(function (response) {
                return response.data.Data;
            });
        }
        var projectTitleList = getProjectList();
        $scope.suggess = function (searchkey) {
            var list = [];
            var value = projectTitleList["$$state"].value;
            for (var i in value) {
                if (indexOf(value[i].projectTitle, searchkey)) {
                    list.push(value[i].projectTitle);
                }
            }
            return list;
        }

        $scope.submit = function (searchkey) {
            $location.path('/search').search({ advance: 'true', searchkey: searchkey });
        }


        function indexOf(obj, searchkey) {
            var str = obj;
            var str2 = searchkey;

            str = str.toLowerCase();
            str = str.replace(/à|á|ạ|ả|ã|â|ầ|ấ|ậ|ẩ|ẫ|ă|ằ|ắ|ặ|ẳ|ẵ/g, "a");
            str = str.replace(/è|é|ẹ|ẻ|ẽ|ê|ề|ế|ệ|ể|ễ/g, "e");
            str = str.replace(/ì|í|ị|ỉ|ĩ/g, "i");
            str = str.replace(/ò|ó|ọ|ỏ|õ|ô|ồ|ố|ộ|ổ|ỗ|ơ|ờ|ớ|ợ|ở|ỡ/g, "o");
            str = str.replace(/ù|ú|ụ|ủ|ũ|ư|ừ|ứ|ự|ử|ữ/g, "u");
            str = str.replace(/ỳ|ý|ỵ|ỷ|ỹ/g, "y");
            str = str.replace(/đ/g, "d");

            str2 = str2.toLowerCase();
            str2 = str2.replace(/à|á|ạ|ả|ã|â|ầ|ấ|ậ|ẩ|ẫ|ă|ằ|ắ|ặ|ẳ|ẵ/g, "a");
            str2 = str2.replace(/è|é|ẹ|ẻ|ẽ|ê|ề|ế|ệ|ể|ễ/g, "e");
            str2 = str2.replace(/ì|í|ị|ỉ|ĩ/g, "i");
            str2 = str2.replace(/ò|ó|ọ|ỏ|õ|ô|ồ|ố|ộ|ổ|ỗ|ơ|ờ|ớ|ợ|ở|ỡ/g, "o");
            str2 = str2.replace(/ù|ú|ụ|ủ|ũ|ư|ừ|ứ|ự|ử|ữ/g, "u");
            str2 = str2.replace(/ỳ|ý|ỵ|ỷ|ỹ/g, "y");
            str2 = str2.replace(/đ/g, "d");
            return str.indexOf(str2) != -1;
        }
    });