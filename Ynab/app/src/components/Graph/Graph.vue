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
    selectedMonth: () => Store.currentMonth
  },
  watch: {
    selectedBudget(newValue) {
      this.loadMonth();
    },
    selectedCategory(newValue) {
      this.reloadGraph();
    },
    selectedMonth(newValue) {
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

      var budgetData = tranformTransactionDataForChart(
        category.budgeted,
        transactions,
        this.currentDate
      );

      var targetData = getTargetDataForChart(
        category.budgeted,
        this.currentDate
      );

      var chart = new Chart(ctx, {
        type: "line",
        data: {
          datasets: [
            {
              label: "Remaining budget",
              backgroundColor: "#0000FF",
              borderColor: "#0000FF",
              fill: false,
              data: budgetData,
              lineTension: 0
            },
            {
              label: "Target",
              backgroundColor: "rgba(255,0,0,255)",
              borderColor: "#FF0000",
              fill: false,
              data: targetData,
              lineTension: 0
            }
          ]
        },
        options: {
          responsive: true,
          title: {
            display: true,
            text: "Spending burdown"
          },
          scales: {
            xAxes: [
              {
                type: "time",
                display: true,
                scaleLabel: {
                  display: true,
                  labelString: "Date"
                },
                ticks: {
                  major: {
                    fontStyle: "bold",
                    fontColor: "#FF0000"
                  }
                }
              }
            ],
            yAxes: [
              {
                display: true,
                scaleLabel: {
                  display: true,
                  labelString: "Budget"
                }
              }
            ]
          }
        }
      });
    },
    prevMonth() {
      this.currentDate = moment(this.currentDate).add(-1, "M");
      this.loadMonth();
    },
    nextMonth() {
      this.currentDate = moment(this.currentDate).add(1, "M");
      this.loadMonth();
    }
  }
};

function tranformTransactionDataForChart(startAmount, transactions, month) {
  let daysInMonth = moment(month).daysInMonth();
  let data = Array(daysInMonth);

  var amount = startAmount;
  for (var i = 0; i < daysInMonth; i++) {
    let date = moment(month).set("D", i + 1);
    let points = transactions.filter(e => moment(e.date).isSame(date, "day"));
    amount += points.reduce((a, b) => (a += b.amount), 0);
    data[i] = {
      x: date.endOf("day").toDate(),
      y: round(amount / 1000, 2)
    };
  }

  return data;
}

function getTargetDataForChart(startAmount, month) {
  let daysInMonth = moment(month).daysInMonth();
  let data = Array(daysInMonth + 1);
  var amount = startAmount;
  var avg = startAmount / daysInMonth;

  data[0] = {
    x: moment(month)
      .set("D", 1)
      .startOf("day")
      .toDate(),
    y: round(amount / 1000, 2)
  };

  for (let i = 1; i < daysInMonth + 1; i++) {
    amount -= avg;
    data[i] = {
      x: moment(month)
        .set("D", i)
        .endOf("day")
        .toDate(),
      y: round(amount / 1000, 2)
    };
  }

  return data;
}

function round(number, decimals = 0) {
  const n = Math.pow(10, decimals);
  return Math.round((number + Number.EPSILON) * n) / n;
}
</script>

<style lang="scss" src="./Graph.scss" />
