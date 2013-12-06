app.service('sessionSvc',
['$http',
function ($http) {

    this.sessionId = 0;
    this.userName = '';
    this.isAuthenticated = false;

    //return sess;
}]);