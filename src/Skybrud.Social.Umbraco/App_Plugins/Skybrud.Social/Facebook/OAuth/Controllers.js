angular.module("umbraco").controller("Skybrud.Social.Facebook.OAuth.Controller", ['$scope', 'editorState', function ($scope, editorState) {

    // Define an alias for the editor (eg. used for callbacks)
    var alias = ('skybrudsocial_' + Math.random()).replace('.', '');

    // Get a reference to the current editor state
    var state = editorState.current;

    $scope.expiresDays = null;
    
    $scope.callback = function (data) {

        $scope.model.value = data;//Diogo: Seta o JSON retornado pelo FacebookOAuth.aspx como valor do MODEL

        //Diogo: Carrega dados quando autoriza pela 1° vez
        updateUI();

        //Diogo: carregar dropDownList() com business Pages
        updateBusinessPagesDropdownUI();
    };


    function updateBusinessPagesDropdownUI() {

        //load itens do COMBO
        var array = $scope.model.value.business_pages;
        if (array) {
            //for (var i = 0; i < array.length; i++) {
            //    console.log(array[i]);
            //}
            //se possui accounts carrega no dropdown
            $scope.accounts = array;
        }


        //TODO: se tiver o ID de uma página salva, carrega os dados no MODEL e seta no combo
        if ($scope.model.value && $scope.model.value.selected_business_page) {
            $scope.selectedBusinessPage = $scope.model.value.selected_business_page;
        }


        //registra EVENTO ao selecionar valor no combo
        $scope.businessPageSelected = function (selectedAccount) {
            $scope.selectedBusinessPage = selectedAccount;//facilidade de acesso na View!
            $scope.model.value.selected_business_page = selectedAccount;//persistência no Umbraco!
        };
    }

    $scope.authorize = function () {

        var url = '/App_Plugins/Skybrud.Social/Dialogs/FacebookOAuth.aspx?callback=' + alias;
        url += "&contentTypeAlias=" + state.contentTypeAlias;
        url += "&propertyAlias=" + $scope.model.alias;

        window.open(url, 'Facebook OAuth', 'scrollbars=no,resizable=yes,menubar=no,width=800,height=600');

    };

    $scope.clear = function () {

        $scope.model.value = null;

    };
    
    function format(number, decimals) {
        var text = Math.round(number * 10 * decimals);
        return text / 10 / decimals;
    }
    
    function updateUI() {

        if ($scope.model.value && $scope.model.value.expires_at) {

            var seconds = (new Date($scope.model.value.expires_at) - new Date()) / 1000;
            $scope.expiresDays = format(seconds / 60 / 60 / 24, 2);

        } else {

            $scope.expiresDays = NaN;

        }

    }

    //entrou na página tenta carregar DADOS salvos
    updateUI();
    updateBusinessPagesDropdownUI();//Diogo:

    // Register the callback function in the global scope
    window[alias] = $scope.callback;

}]);

angular.module("umbraco").controller("Skybrud.Social.Facebook.OAuth.PreValues.Controller", ['$scope', 'assetsService', function ($scope, assetsService) {
    
    if (!$scope.model.value) {
        $scope.model.value = {
            appid: '',
            appsecret: '',
            redirecturi: ''
        };
    }

    $scope.suggestedRedirectUri = window.location.origin + '/App_Plugins/Skybrud.Social/Dialogs/FacebookOAuth.aspx';

    assetsService.loadCss("/App_Plugins/Skybrud.Social/Social.css");

}]);