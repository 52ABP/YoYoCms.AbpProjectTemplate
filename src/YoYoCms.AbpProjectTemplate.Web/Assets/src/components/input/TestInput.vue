<style rel="styleesheet" lang="scss">
</style>

<template>
    <article>
        <input type="text" @input="testinput" @keyup.enter="keyup">
    </article>
</template>

<script>
    export default {
        props: {
            type: [String, Number]
        },
        data() {
            return {}
        },
        created() {
        },
        mounted() {
            console.log(this.$props)
        },
        activated() {
        },
        methods: {
            testinput(e) {
                console.log(e.target.value)
            },
            keyup(e) {
                this.$emit('testfn', e)
            }
        },
        components: {}
    }
</script>
