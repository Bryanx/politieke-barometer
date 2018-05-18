var jsonResult;

Vue.use(VueFormWizard);
Vue.use(VueFormGenerator);
new Vue({
    el: '#app',
    data: {
        model: {
            ItemName: '',
            PropertyTag: '',
            SecondItemName: '',
            GraphType: '',
            Title: ''
        },
        formOptions: {
            validationErrorClass: "has-error",
            validationSuccessClass: "has-success",
            validateAfterChanged: true
        },
        firstTabSchema: {
            fields: [
                {
                    type: "input",
                    inputType: "text",
                    id: "basegraph",
                    label: Resources.BasicGraph + "*",
                    model: "ItemName",
                    placeholder: Resources.ChoosePersonOrgTopic,
                    required: true,
                    validator: VueFormGenerator.validators.string,
                    styleClasses: 'col-xs-12'
                },
                { 
                    type: "select",
                    label: Resources.IntersectValue + "*",
                    model: "PropertyTag",
                    required: true,
                    validator: VueFormGenerator.validators.string,
                    values: ['Number of mentions', 'Age', 'Gender', "Personality", "Education"],
                    styleClasses: 'col-xs-12',
                    default: 'Number of mentions',
                },
                {
                    type: "input",
                    inputType: "text",
                    id: "comparinggraph",
                    label: Resources.ComparingGraph + " (" + Resources.Optional + ")",
                    model: "SecondItemName",
                    placeholder: Resources.ChoosePersonOrgTopic,
                    required: false,
                    validator: VueFormGenerator.validators.string,
                    styleClasses: 'col-xs-12'
                }
            ]
        },
        secondTabSchema: {
            fields: [
                {
                    type: "select",
                    inputType: "text",
                    label: Resources.GraphType + "*",
                    model: "GraphType",
                    required: true,
                    validator: VueFormGenerator.validators.string,
                    values: ['Line chart', 'Bar chart', 'Pie chart', 'Donut chart'],
                    styleClasses: 'col-xs-12'
                }
            ]
        },
        thirdTabSchema: {
            fields: [
                {
                    type: "input",
                    inputType: "text",
                    label: Resources.Title,
                    model: "Title",
                    placeholder: Resources.Title,
                    required: false,
                    validator: VueFormGenerator.validators.string,
                    styleClasses: 'col-xs-12'
                }
            ]
        }
    },
    methods: {
        onComplete: function () {
            jsonResult.ItemIds = [];
            jsonResult.ItemIds.push($(".wizardItemId1").html());
            jsonResult.ItemIds.push($(".wizardItemId2").html());
            jsonResult.GraphType = convertWizardGraphTypeToChartType(jsonResult.GraphType);
            addWidgetToDashboard(jsonResult);
        },
        validateFirstTab: function () {
            return this.$refs.firstTabForm.validate();
        },
        validateSecondTab: function () {
            return this.$refs.secondTabForm.validate();
        },
        validateThirdTab: function () {
            return this.$refs.thirdTabForm.validate();
        },
        prettyJSON: function (result) {
            jsonResult = result;
        }
    }
});

(() => {

    //Search function in wizard
    function addAutocomplete() {
        if (searchlist.length > 0) {
            $('#comparinggraph').devbridgeAutocomplete({
                width: 400,
                lookup: searchlist,
                triggerSelectOnValidInput: false,
                maxHeight: 200,
                formatResult: function (suggestion) {
                    return "<div class='compareSuggestion'>" + suggestion.value + "</div>";
                },
                onSelect: function (suggestion) {
                    $(".wizardItemId2").html(suggestion.data);
                }
            });
            $('#basegraph').devbridgeAutocomplete({
                width: 400,
                lookup: searchlist,
                triggerSelectOnValidInput: false,
                maxHeight: 200,
                formatResult: function (suggestion) {
                    return "<div class='compareSuggestion'>" + suggestion.value + "</div>";
                },
                onSelect: function (suggestion) {
                    $(".wizardItemId1").html(suggestion.data);
                }
            });
        } else {
            //Keep trying to get the searchlist 
            setTimeout(addAutocomplete, 100);
        }
    }

    addAutocomplete();

    $(document).on("keyup", "#basegraph", (e) => $($('.compareSuggestion')[0]).parent().parent().css("margin-left", "0"));
    $(document).on("keyup", "#comparinggraph", (e) => $($('.compareSuggestion')[0]).parent().parent().css("margin-left", "0"));

})(jQuery);