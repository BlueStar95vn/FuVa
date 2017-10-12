angular.
    module('teamAdd', ['ngRoute']).
    controller("addTeamCtrl", function ($scope) {
        $scope.members = [];
        $scope.addMember = function () {
            $scope.members.push({ 'name': $scope.newMember });
            $scope.newMember = '';
        }
        $scope.delMember = function (index) {
            $scope.members.splice(index, 1);

        }
    });
    