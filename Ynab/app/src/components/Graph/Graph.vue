<template>
  <div class="app__graph z-level-1">
    <div class="date-controls">
      <div class="prev">
        <md-button class="md-icon-button md-raised md-primary" @click="prevMonth">
          <md-icon>arrow_back</md-icon>
        </md-button>
      </div>
      <div class="center-text">
        <div>{{ monthText }}</div>
        <div>{{ yearText }}</div>
      </div>
      <div class="next">
        <md-button class="md-icon-button md-raised md-primary" @click="nextMonth">
          <md-icon>arrow_forward</md-icon>
        </md-button>
      </div>
    </div>
    <div class="graph-container">
      <div v-if="loading && active" class="loading">
        <md-progress-spinner md-mode="indeterminate"></md-progress-spinner>
      </div>
      <canvas v-else-if="active" class="graph" ref="graph"></canvas>
    </div>
  </div>
</template>

<script>
import moment, { duration } from "moment";
import "chartjs-adapter-moment";
import Chart from "chart.js";
import Store from "src/services/store.service";
import Api from "src/services/api.service";

export default {
  name: "Graph",
  data() {
    return {
      currentDate: moment(),
      loading: false,
      active: true,
      graph: null
    };
  },
  computed: {
    monthText() {
      return moment(this.currentDate).format("MMMM");
    },
    yearText() {
      return moment(this.currentDate).format("yyyy");
    },
    selectedBudget() {
      return Store.currentBudget;
    },
    selectedCategory() {
      return Store.currentCategory;
    },
    selectedMonth() {
      return Store.selectedMonth;
    }
  },
  watch: {
    selectedBudget(newValue) {
      this.loadMonth();
    },
    selectedCategory(newValue) {
      this.reloadGraph();
    },
    currentDate(newValue) {
      this.reloadGraph();
    }
  },
  methods: {
    loadMonth() {
      if (!this.selectedBudget) return;
      Api.getMonth(this.selectedBudget, this.currentDate).then(month => {
        Store.currentMonth = month;
      });
    },
    reloadGraph() {
      if (this.selectedBudget && this.selectedCategory && Store.currentMonth) {
        this.loading = true;

        Api.getTransactions(
          this.selectedBudget,
          this.selectedCategory,
          this.currentDate
        ).then(transactions => {
          this.loading = false;
          return this.$nextTick().then(() => this.buildGraph(transactions));
        });
      }
    },
    buildGraph(transactions) {
      var category = Store.getCategory(Store.currentCategory);

      var ctx = this.$refs.graph;

      var chartData = tranformTransactionDataForChart(
        category.budgeted,
        transactions,
        this.currentDate
      );

      console.log(chartData);

      var chart = new Chart(ctx, {
        type: "line",
        responsive: true,
        data: {
          datasets: [
            {
              label: "Spending",
              color: "#f00",
              data: chartData
            }
          ]
        },
        options: {
          title: {
            text: "Chart.js Time Scale"
          },
          scales: {
            x: {
              type: "time",
              time: {},
              scaleLabel: {
                display: true,
                labelString: "x"
              }
            },
            y: {
              scaleLabel: {
                display: true,
                labelString: "y"
              }
            }
          }
        }
      });
    },
    prevMonth() {
      this.currentDate = moment(this.currentDate).add(-1, "M");
    },
    nextMonth() {
      this.currentDate = moment(this.currentDate).add(1, "M");
    }
  }
};

function tranformTransactionDataForChart(startAmount, transactions, month) {
  let daysInMonth = moment(month).daysInMonth();
  let data = Array(daysInMonth);

  var amount = startAmount;
  for (var i = 1; i <= daysInMonth; i++) {
    let date = moment(month).set("D", i);
    let points = transactions.filter(e => moment(e.date).isSame(date, "day"));
    amount += points.reduce((a, b) => (a += b.amount), 0);
    data[i] = {
      x: date,
      y: amount
    };
  }

  return data;
}
</script>

<style lang="scss" src="./Graph.scss" />
