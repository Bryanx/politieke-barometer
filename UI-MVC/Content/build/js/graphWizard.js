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
            SecondPropertyTag: '',
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
                    label: "Basis grafiek*",
                    model: "ItemName",
                    placeholder: "Kies een politicus partij of thema",
                    required: true,
                    validator: VueFormGenerator.validators.string,
                    styleClasses: 'col-xs-12'
                },
                {
                    type: "select",
                    label: "Kruising waarde*",
                    model: "PropertyTag",
                    required: true,
                    validator: VueFormGenerator.validators.string,
                    values: ['Number of mentions', 'Age', 'Gender'],
                    styleClasses: 'col-xs-12'
                },
                {
                    type: "input",
                    inputType: "text",
                    label: "Vergelijkende grafiek (optioneel)",
                    model: "SecondItemName",
                    placeholder: "Kies een politicus partij of thema",
                    required: false,
                    validator: VueFormGenerator.validators.string,
                    styleClasses: 'col-xs-12'
                },
                {
                    type: "select",
                    label: "Kruising waarde",
                    model: "SecondPropertyTag",
                    required: false,
                    validator: VueFormGenerator.validators.string,
                    values: ['United Kingdom', 'Romania', 'Germany'],
                    styleClasses: 'col-xs-12'
                }
            ]
        },
        secondTabSchema: {
            fields: [
                {
                    type: "select",
                    inputType: "text",
                    label: "Grafiek type*",
                    model: "GraphType",
                    required: true,
                    validator: VueFormGenerator.validators.string,
                    values: ['line', 'bar', 'pie', 'donut'],
                    styleClasses: 'col-xs-12'
                },
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
            addWidgetToDashboard(jsonResult);
        },
        validateFirstTab: function () {
            return this.$refs.firstTabForm.validate();
        },
        validateSecondTab: function () {
            return this.$refs.secondTabForm.validate();
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
            $('#vergelijkende-grafiek-optioneel').devbridgeAutocomplete({
                width: 400,
                lookup: searchlist,
                triggerSelectOnValidInput: false,
                maxHeight: 200,
                formatResult: function (suggestion) {
                    return "<div class='compareSuggestion'>" + suggestion.value + "</div>";
                },
                onSelect: function (suggestion) {
                    // OK
                }
            });
            $('#basis-grafiek').devbridgeAutocomplete({
                width: 400,
                lookup: searchlist,
                triggerSelectOnValidInput: false,
                maxHeight: 200,
                formatResult: function (suggestion) {
                    return "<div class='compareSuggestion'>" + suggestion.value + "</div>";
                },
                onSelect: function (suggestion) {
                    // OK
                }
            });
        } else {
            //Keep trying to get the searchlist 
            setTimeout(addAutocomplete, 100);
        }
    }

    addAutocomplete();

    $(document).on("keyup", "#basis-grafiek", (e) => $($('.compareSuggestion')[0]).parent().parent().css("margin-left", "0"));
    $(document).on("keyup", "#vergelijkende-grafiek-optioneel", (e) => $($('.compareSuggestion')[0]).parent().parent().css("margin-left", "0"));

})(jQuery);