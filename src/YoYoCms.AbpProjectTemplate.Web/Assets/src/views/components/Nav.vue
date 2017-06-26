<!--首页导航-->
<style rel="styleesheet" lang="scss">
</style>

<template>
    <ol class="breadcrumb breadcrumb-col-cyan" style="background: none;padding-left: 0">

        <li v-for="(item,index) in nav">
            <i class="material-icons">{{icons[index]}}</i> {{item.displayName}}
        </li>
    </ol>
</template>

<script>
    export default {
        data() {
            return {
                nav: this.$store.state.index.navShow,
                icons: ['home', 'library_books', 'archive', 'attachment', 'extension']
            }
        },
        watch: {
            '$store.state.index.navShow'(val) {
                this.nav = val
            }
        },
        created() {
        },
        activated() {
        },
        components: {}
    }
</script>
