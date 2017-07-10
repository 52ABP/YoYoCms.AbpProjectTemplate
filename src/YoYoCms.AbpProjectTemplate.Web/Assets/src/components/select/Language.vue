<!--所有语言和国旗-->
<style lang="scss" rel="stylesheet">
    .select__language-container {
        section {
            margin-bottom: 5px;
            width: 60%;
            > i {
                width: 5em;
                display: inline-block;
                text-align: right;
                margin-right: 5px;
            }

            > h5 {
                vertical-align: middle;
            }
        }
    }
</style>

<template>
    <article class="select__language-container" v-loading="loading">
        <section>
            <i>{{L('Languages')}}</i>
            <el-select v-model="currVal" :placeholder="L('Select')" filterable>
                <el-option
                        v-for="item in data"
                        :label="item.displayText"
                        :key="item.value"
                        :value="item.value">
                </el-option>
            </el-select>
        </section>

        <section>
            <i>{{L('Flag')}}</i>
            <el-select v-model="flagVal" :placeholder="L('Select')" filterable>
                <el-option
                        v-for="item in flagData"
                        :label="item.displayText"
                        :key="item.displayText"
                        :value="item.value">
                    <i :class="item.value"></i> {{item.displayText}}
                </el-option>
            </el-select>
            <h5 :class="flagVal"></h5>
        </section>
    </article>
</template>

<script>
    import langService from '../../services/common/languageService'
    export default{
        props: {
            value: Object, // 包含name和icon 字段
            onChange: Function,
            flag: '',
        },
        data () {
            return {
                loading: true,
                data: [],
                currVal: void 0,

                flagData: [],
                flagVal: void 0,
            }
        },
        watch: {
            'value'(val) {
                if (val.name == this.currVal) return
                this.currVal = val.name
                setTimeout(() => {
//                    debugger
                    this.flagVal = val.icon
                }, 0)
            },
            'currVal' (val) {
//                this.$emit('input', val)
                this.flagVal = {}
                if (!val) return
                for (let i = 0; i < this.flagData.length; i++) {
                    let item = this.flagData[i]
                    if (item.displayText.toLowerCase().indexOf(val.toLowerCase()) > -1 || val.toLowerCase().toLowerCase().indexOf(item.displayText) > -1) {
                        this.flagVal = item.value
                        break
                    }
                }
                this.$emit('input', Object.assign({}, this.value, {name: val}))
            },
            'flagVal'(val) {
                this.$emit('input', Object.assign({}, this.value, {icon: val}))
            },
        },
        async created () {
            await this.fetchData()
            this.currVal = this.value.name
        },
        activated () {
        },
        methods: {
            async fetchData () {
                this.loading = true
                let ret = await langService.getLanguageForEdit({id: null})

                this.data = ret.languageNames
                this.flagData = ret.flags
                setTimeout(() => {
                    this.loading = false
                }, 500)
            },
            handleChange() {
                this.onChange && this.onChange(this.currVal)
            },
        },
        components: {}
    }
</script>
