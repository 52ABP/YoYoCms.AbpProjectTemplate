<style rel="styleesheet" lang="scss">
    .loginregister__languages-container {
        > section {
            display: inline-block;
            margin-left: 5px;
            cursor: pointer;
        }
    }
</style>

<template>
    <article class="loginregister__languages-container" v-loading="loading">
        <section v-for="item in langs" class="waves-effect">
            <i :class="item.icon" :title="item.displayName" @click="choose(item)"></i>
        </section>
    </article>
</template>

<script>
    import languageService from '../../../services/common/languageService'
    export default {
        data() {
            return {
                loading: false,
                currLang: abp.localization.currentLanguage,
                langs: abp.localization.languages
            }
        },
        created() {
        },
        activated() {
        },
        methods: {
            async choose(item) {
                if (item.name === this.currLang.name) return
                this.loading = true
                await languageService.changeLanguage(item.name)
                window.location.reload()
                this.loading = false
            }
        },
        components: {}
    }
</script>
