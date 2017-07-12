<style rel="styleesheet" lang="scss">
</style>

<template>
    <article>
        <InputTest @keyup.enter.native="testfn" type="password"></InputTest>
    </article>
</template>

<script>
    import InputTest from '../components/input/TestInput.vue'
    export default {
        data() {
            return {}
        },
        created() {
        },
        activated() {
        },
        methods: {
            testfn (val) {
                console.log(val)
            }
        },
        components: {
            InputTest
        }
    }
</script>
