<style rel="styleesheet" lang="scss">
    @import "common/style";
    @import "./vendor/flags/famfamfam-flags.css";
</style>
<template>
    <div id="app">
        <router-view></router-view>
    </div>
</template>

<script>
    import store from './store'
    import './filters/timeFilter'
    export default {
        name: 'app',
        store,
    }
</script>
