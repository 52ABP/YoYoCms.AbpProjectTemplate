<style rel="styleesheet" lang="scss">
    img {
        width: 10px;
    }
</style>

<template>
    <article>
        <select ref="sel" style="min-width: 200px" class="selectpicker">
            <i ref="item"><img src="https://select2.github.io/vendor/images/flags/hi.png"> 111</i>
            <option v-for="item in data" :value="item[valKey]">
                :data-content='$refs.item.innerHTML'>
            </option>
        </select>
    </article>
</template>

<script>
    import '../../../vendor/boot-select/js/bootstrap-select'
    import '../../../vendor/boot-select/css/bootstrap-select.min.css'
    //    import Vue from 'vue'
    //
    //    Vue.directive('my-directive', {
    //        bind: function () {},
    //        inserted: function () {},
    //        update: function () {},
    //        componentUpdated: function () {},
    //        unbind: function () {}
    //    })

    export default {
        props: {
            value: String | Number,
            data: Array,
            textKey: {
                type: String,
                default: 'key'
            },
            valKey: {
                type: String,
                default: 'value'
            }
        },
        data() {
            return {
                currVal: this.value
            }
        },
        watch: {
            'value'(val) {
                if (val != this.currVal) {
                    this.currVal = val
//                    $(this.$refs.sel).val(222).trigger('change')
                }
            },
            'currVal'(val) {
                this.$emit('input', val)
            }
        },
        created() {
        },
        mounted() {
            console.log(this.$refs.item.innerHTML)
            $(this.$refs.sel).selectpicker()
        },
        activated() {
        },
        methods: {},
        components: {}
    }
</script>
