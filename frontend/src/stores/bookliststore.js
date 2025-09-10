import { defineStore } from 'pinia'
import {
  createBooklist as apiCreateBooklist,
  deleteBooklist as apiDeleteBooklist,
  getBooklistDetails as apiGetBooklistDetails,
  recommendBooklists as apiRecommendBooklists,
  addBookToBooklist as apiAddBookToBooklist,
  removeBookFromBooklist as apiRemoveBookFromBooklist,
  collectBooklist as apiCollectBooklist,
  cancelCollectBooklist as apiCancelCollectBooklist,
  updateCollectNotes as apiUpdateCollectNotes,
  searchBooklistsByReader as apiSearchBooklistsByReader,
  updateBooklistName as apiUpdateBooklistName,
  updateBooklistIntro as apiUpdateBooklistIntro
} from '@/modules/book/api.js'

function pickError(err) {
  return err?.response?.data?.message || err?.message || '请求失败'
}

export const useBooklistStore = defineStore('booklist', {
  state: () => ({
    booklists: { created: [], collected: [] },
    currentBooklist: null,   // 对应 GetBooklistDetailsResponse
    recommended: [],         // RecommendBooklistsResponse.items
    loading: false,
    error: null,
    lastReaderId: null       // 用于收藏/取消收藏后便捷刷新
  }),

  getters: {
    created: (s) => s.booklists.created || [],
    collected: (s) => s.booklists.collected || [],
    // 合并视图
    allForDisplay: (s) => {
      const created = (s.booklists.created || []).map(x => ({ ...x, _source: 'created' }))
      const collected = (s.booklists.collected || []).map(x => ({ ...x, _source: 'collected' }))
      return [...created, ...collected]
    }
  },

  actions: {
    clearError() { this.error = null },
    clearCurrent() { this.currentBooklist = null },

    async createBooklist(payload) {
      this.clearError()
      try {
        this.loading = true
        // 创建书单（后端返回 { booklistId, success }）
        const { data } = await apiCreateBooklist(payload)
        if (this.lastReaderId != null) {
          try {
            await this.fetchBooklistsByReader(this.lastReaderId)
          } catch (refreshErr) {
            this.error = this.error || pickError(refreshErr)
          }
        }
        return data
      } catch (err) {
        this.error = pickError(err)
        throw err
      } finally {
        this.loading = false
      }
    },

    async deleteBooklist(booklistId) {
      this.clearError()
      try {
        this.loading = true
        await apiDeleteBooklist(booklistId)
        this.booklists = {
          created: (this.booklists.created || []).filter(b => b.booklistId !== booklistId),
          collected: (this.booklists.collected || []).filter(b => b.booklistId !== booklistId)
        }
        if (this.currentBooklist?.booklistInfo?.booklistId === booklistId) {
          this.currentBooklist = null
        }
      } catch (err) {
        this.error = pickError(err)
        throw err
      } finally {
        this.loading = false
      }
    },

    async fetchBooklistDetails(booklistId) {
      this.clearError()
      try {
        this.loading = true
        const { data } = await apiGetBooklistDetails(booklistId)
        // data 结构：{ booklistInfo, books }
        this.currentBooklist = data
        return data
      } catch (err) {
        this.error = pickError(err)
        throw err
      } finally {
        this.loading = false
      }
    },

    async fetchRecommended(booklistId, limit = 10) {
      this.clearError()
      try {
        this.loading = true
        const { data } = await apiRecommendBooklists(booklistId, limit)
        // ASP.NET 默认 camelCase => items
        this.recommended = data.items || []
        return this.recommended
      } catch (err) {
        this.error = pickError(err)
        throw err
      } finally {
        this.loading = false
      }
    },

    async addBook(booklistId, payload) {
      this.clearError()
      try {
        await apiAddBookToBooklist(booklistId, payload)
        // 为保证数据一致，直接重新拉取详情
        if (this.currentBooklist?.booklistInfo?.booklistId === booklistId) {
          await this.fetchBooklistDetails(booklistId)
        }
      } catch (err) {
        this.error = pickError(err)
        throw err
      }
    },

    async removeBook(booklistId, isbn) {
      this.clearError()
      try {
        await apiRemoveBookFromBooklist(booklistId, isbn)
        if (this.currentBooklist?.booklistInfo?.booklistId === booklistId) {
          await this.fetchBooklistDetails(booklistId)
        }
      } catch (err) {
        this.error = pickError(err)
        throw err
      }
    },

    async collect(booklistId, payload) {
      this.clearError()
      try {
        await apiCollectBooklist(booklistId, payload)
        if (this.lastReaderId != null) {
          await this.fetchBooklistsByReader(this.lastReaderId)
        }
      } catch (err) {
        this.error = pickError(err)
        throw err
      }
    },

    async cancelCollect(booklistId) {
      this.clearError()
      try {
        await apiCancelCollectBooklist(booklistId)
        if (this.lastReaderId != null) {
          await this.fetchBooklistsByReader(this.lastReaderId)
        }
      } catch (err) {
        this.error = pickError(err)
        throw err
      }
    },

    async updateCollectNotes(booklistId, payload) {
      this.clearError()
      try {
        await apiUpdateCollectNotes(booklistId, payload)
        // 若当前详情页就是该书单，刷新详情
        if (this.currentBooklist?.booklistInfo?.booklistId === booklistId) {
          await this.fetchBooklistDetails(booklistId)
        }
      } catch (err) {
        this.error = pickError(err)
        throw err
      }
    },

    async fetchBooklistsByReader(readerId) {
      this.clearError()
      try {
        this.loading = true
        const { data } = await apiSearchBooklistsByReader(readerId)
        // data: { created: SimpleBooklistDto[], collected: SimpleBooklistDto[] }
        this.booklists = {
          created: data.created || [],
          collected: data.collected || []
        }
        this.lastReaderId = readerId
        return this.booklists
      } catch (err) {
        this.error = pickError(err)
        throw err
      } finally {
        this.loading = false
      }
    },

    async updateName(booklistId, payload) {
      this.clearError()
      try {
        await apiUpdateBooklistName(booklistId, payload)
        // 刷新详情
        if (this.currentBooklist?.booklistInfo?.booklistId === booklistId) {
          await this.fetchBooklistDetails(booklistId)
        }
        // 同步更新列表中的显示
        this.booklists = {
          created: (this.booklists.created || []).map(b => b.booklistId === booklistId ? { ...b, booklistName: payload.newName } : b),
          collected: (this.booklists.collected || []).map(b => b.booklistId === booklistId ? { ...b, booklistName: payload.newName } : b)
        }
      } catch (err) {
        this.error = pickError(err)
        throw err
      }
    },

    async updateIntro(booklistId, payload) {
      this.clearError()
      try {
        await apiUpdateBooklistIntro(booklistId, payload)
        if (this.currentBooklist?.booklistInfo?.booklistId === booklistId) {
          await this.fetchBooklistDetails(booklistId)
        }
        this.booklists = {
          created: (this.booklists.created || []).map(b => b.booklistId === booklistId ? { ...b, booklistIntroduction: payload.newIntro ?? null } : b),
          collected: (this.booklists.collected || []).map(b => b.booklistId === booklistId ? { ...b, booklistIntroduction: payload.newIntro ?? null } : b)
        }
      } catch (err) {
        this.error = pickError(err)
        throw err
      }
    }
  }
})
