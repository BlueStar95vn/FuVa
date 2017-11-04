angular.
    module('teamEdit').
    component('teamEdit', {
        templateUrl: '/app/teams/team-edit/team-edit.html',
        controller: function ($http,$routeParams,$location) {
            var vm = this;

            vm.teamNameModel = {};
            vm.teamLeadModel = {};
            vm.newMember = {};  
            //Load team info
            $http.get('http://localhost:63237/api/teams/getteamdetail/' + $routeParams.teamId).then(function (response) {
                vm.teamNameModel = response.data;
                vm.teamLeadModel = response.data;
            }).catch(function (err){
                console.log(err);
                });

           
            //Load member in team
            $http.get('http://localhost:63237/api/teams/getteammember/' + $routeParams.teamId).then(function (response) {
                vm.members = response.data;
            });
            //Reload member list in team
            vm.refreshMember = function () {
                $http.get('http://localhost:63237/api/teams/getteammember/' + $routeParams.teamId).then(function (response) {
                    vm.members = response.data;
                });
            };

            //verify changes
            vm.teamNameSaved = false;
          
            vm.teamNameOnChanging = function () {
                vm.teamNameSaved = false;
            }
            
            //change team name
            vm.changeTeamName = function () {
                $http.post('http://localhost:63237/api/teams/changeteamname/' + $routeParams.teamId, vm.teamNameModel).then(function () {
                    vm.teamNameSaved = true;
                });               
            };

             //Change Team Lead
            vm.showTeamLead = false;
            vm.showChangeTeamLead = function () {
                vm.showTeamLead = true;
            };
            vm.hideChangeTeamLead = function () {
                vm.showTeamLead = false;
            }
            vm.changeTeamLead = function () {
                $http.post('http://localhost:63237/api/teams/changeteamlead/' + $routeParams.teamId, vm.teamLeadModel).then(function () {
                    vm.showTeamLead = false;
                });  
               
            };

            vm.employeeList = [];
            //Load all employees to add member % change team lead
            $http.get('http://localhost:63237/api/employees/getall').then(function (response) {
                vm.employeeList = response.data;
                vm.newMember = vm.employeeList[0];
            });
                     
            vm.addMember = function () {
                $http.post('http://localhost:63237/api/teams/addmember/' + $routeParams.teamId, vm.newMember).then(function (response) {
                    vm.refreshMember();
                }).catch(function (err) {
                    console.log(err);
                });
            };

            //remove member
            vm.removeMember = function (id, index) {
                $http.delete('http://localhost:63237/api/teams/deletemember/' + id).then(function (response) {
                    vm.members.splice(index, 1);
                }).catch(function (err) {
                    console.log(err);
                });
            }
        }
    });
    