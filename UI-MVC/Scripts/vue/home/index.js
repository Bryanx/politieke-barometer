import Vue from 'vue'
import FirstComponent from './FirstComponent.vue'


new Vue({
    el: '#app',
    components: {
        FirstComponent
    },
    data(){
        return {
            vueMessage: 'Hello'
        }
    }
});