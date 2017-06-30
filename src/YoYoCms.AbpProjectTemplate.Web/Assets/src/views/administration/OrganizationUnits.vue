<!--组织机构-->
<style rel="styleesheet" lang="scss">
    @import "../../mixins/topSearch";
    @import "../../mixins/common";

    .administration-organizationUnits-container {
        @extend %content-container;

        .search {
            @extend %top-search-container;
        }
    }
</style>

<template>
    <article class="administration-organizationUnits-container" v-loading="loading">
        <JsTree ref="jstree" :treeData="treeData" :onDragStop="dragStop"
                :plugins="['dnd','types', 'wholerow']"></JsTree>
    </article>
</template>

<script>
    import JsTree from '../../components/tree/JsTree.vue'
    import organizationUnitService from '../../services/organizationUnitService'
    export default {
        data() {
            return {
                treeData: [],
                loading: false
            }
        },
        created() {
        },
        async activated() {
            this.fetchData()
        },
        methods: {
            async fetchData () {
                this.loading = true
                let treeData = (await organizationUnitService.getOrganizationUnits()).items
                treeData.map((item) => {
                    item.text = `${item.displayName} (${item.memberCount})`
                    item.parent = item.parentId || '#'
                    item.state = {opened: true}
                })
                this.treeData = treeData
                this.$refs.jstree.init()
                this.loading = false
            },
            //
            dragStop (id, newParentId) {
                abp.message.confirm('是否确认移动?', async (ret) => {
                    if (!ret) {
                        this.$refs.jstree.init()
                        return
                    }

                    try {
                        this.loading = true
                        await organizationUnitService.moveOrganizationUnit({id, newParentId})
                        abp.notify.success('操作成功!', '恭喜')
                    } finally {
                        this.loading = false
                    }
                })
            }
        },
        components: {JsTree}
    }
</script>
