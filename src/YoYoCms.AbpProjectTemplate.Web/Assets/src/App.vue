<style rel="styleesheet" lang="scss">
    @import "./vendor/flags/famfamfam-flags.css";
    @import './mixins/_mixins.scss';
</style>
<template>
    <div id="app">
        <router-view></router-view>
    </div>
</template>

<script>
    import './vendor/material-icons/material-icons.scss'
    import './common/style.scss'

    import './vendor/abp/scripts/libs/sweetalert/sweetalert.scss'
    import './vendor/bsb/scss/style.scss'
    import './vendor/bsb/css/themes/all-themes.min.css'
    import './vendor/bsb/plugin/node-waves/waves.scss'
    import './mixins/_common.scss'

    import store from './store/index.js'
    import './filters/timeFilter'

    import './vendor/bsb/js/admin'
    export default {
        name: 'app',
        store,
    }
</script>
