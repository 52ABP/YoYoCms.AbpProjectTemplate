<style rel="styleesheet" lang="scss">
</style>

<template>
    <el-dropdown trigger="click">
        <span class="el-dropdown-link" style="cursor:pointer;">
            <i :class="currLang.icon"></i> {{currLang.displayName}}
            <i style="vertical-align: middle" class="el-icon-caret-bottom el-icon--right"></i>
        </span>
        <el-dropdown-menu slot="dropdown">
            <el-dropdown-item v-for="item in langs" :key="item.displayName">
                <div @click="choose(item)"><i :class="item.icon"></i> {{item.displayName}}</div>
            </el-dropdown-item>
        </el-dropdown-menu>
    </el-dropdown>
</template>

<script>
    import languageService from '../../services/common/languageService'
    export default {
        data() {
            return {
                currLang: abp.localization.currentLanguage,
                langs: abp.localization.languages
            }
        },
        created() {
            console.log(this.currLang)
        },
        activated() {
        },
        methods: {
            async choose(item) {
                if (item.name === this.currLang.name) return
                await languageService.changeLanguage(item.name)
                window.location.reload()
            }
        },
        components: {}
    }
</script>
