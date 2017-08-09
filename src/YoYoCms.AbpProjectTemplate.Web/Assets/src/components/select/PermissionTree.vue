<!--权限树的下拉框-->
<style rel="styleesheet" lang="scss">
    .permissiontree--container {
        overflow: visible;

        .box-card {
            position: absolute;
            z-index: 200;
            max-height: 500px;
            overflow-y: scroll;
        }
    }
</style>

<template>
    <article class="permissiontree--container" ref="container">
        <el-input :placeholder="L('Permissions')" @focus="!showList && (showList=true)" ref="input" v-bind:value="currVal.text"
                  :icon="currVal.id ? 'circle-close' : ''"
                  :on-icon-click="() => { currVal = {} }"
                  :readonly="true"></el-input>
        <el-card class="box-card" v-show="showList">
            <JsTree :treeData="treeData" ref="jsTree" :context.sync="jsTreeVm" :onItemClick="itemClick"></JsTree>
        </el-card>
    </article>
</template>

<script>
    import userService from '../../services/userService'
    import JsTree from '../tree/JsTree.vue'
    export default {
        props: {
            onChange: Function, // 当选择变化
            value: Object,
        },
        data() {
            return {
                treeData: [],
                loading: false,
                userid: void 0,
                showList: false, // 显示权限列表
                jsTreeVm: {}, // jstree的上下文
                currVal: this.value || {}
            }
        },
        watch: {
            'showList' (val) {
                if (val) {
                    window.addEventListener('click', this.hideList)
                    this.init()
                } else window.removeEventListener('click', this.hideList)
            },
            'value' (val) {
                if (val !== this.currVal) this.currVal = val
            },
            'currVal' (val) {
                this.$emit('input', val)
                this.onChange && this.onChange(val)
            }
        },
        async activated() {
        },
        deactivated () {
            window.removeEventListener('click', this.hideList)
        },
        beforeDestroy () {
            window.removeEventListener('click', this.hideList)
        },
        mounted() {
        },
        methods: {
            async init () {
                this.userid = this.$store.state.auth.user.id
                // 如果有数据 则不要重复加载
                if (this.treeData.length > 0) return

                this.loading = true
                let retPermission = (await userService.getUserPermissionsForEdit({id: this.userid})).permissions
                retPermission.forEach((item) => {
                    this.treeData.push({
                        id: item.name,
                        parent: item.parentName ? item.parentName : '#',
                        text: item.displayName,
                        state: {
                            opened: true,
                        }
                    })
                })

                this.$nextTick(() => {
                    this.jsTreeVm.init()
                })
            },
            hideList (e) {
                for (let k in e.path) {
                    let item = e.path[k]
                    if (item === this.$refs.container) return
                }
                this.showList = false
            },
            itemClick (item) {
                this.showList = false
                if (item.id !== this.currVal.id) this.currVal = item
            }
        },
        components: {JsTree}
    }
</script>
