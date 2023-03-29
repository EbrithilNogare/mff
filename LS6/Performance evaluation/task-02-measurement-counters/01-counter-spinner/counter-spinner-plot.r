my.printf <- function(fmt, ...) {
    writeLines(sprintf(fmt=fmt, ...), sep="")
}

my.img.open <- function(filename, width=11.69, height=8.26, pointsize=10) {
    last.filename <<- filename

    pdf(last.filename,
            width=width, height=height,
            pointsize=pointsize,
    )
    par(font.main=1)
}

my.img.close <- function() {
    xxx <- dev.off()
}


my.stress.plot <- function(x) {
    my.plot <- function(x, ylab, ...) {
        plot(x, type="l", ylab=ylab, xlab="Index", frame.plot=F, ...)
    }

    my.get.lims <- function(x) {
        res <- c( min(x), max(x) )
        if (is.na(res[1]) || is.na(res[2])) {
            res <- c(0, 1)
        }
        if (is.infinite(res[1])) {
            res[1] <- 0
        }
        if (is.infinite(res[2])) {
            res[2] <- res[1] * 100 + 0.01
        }
        if (res[1] == res[2]) {
            res <- c(res[1], res[2] + 0.001)
        }
        res
    }

    my.cmp.plot <- function(x.work, x.empty, ylab, ...) {
        plot(c(), c(), ylim=my.get.lims(c(x.work, x.empty)), xlim=c(1,length(x.work)), type="l", ylab=ylab, xlab="Index", frame.plot=F, ...)
        lines(x.work, col="red")
        lines(x.empty, col="green")
    }

    x.work <- subset(x, x$type == "work")
    x.empty  <- subset(x, x$type == "empty")

    if ((length(x.work$index) != length(x.empty$index)) || (length(x.empty$index) == 0)) {
        my.printf("\nERROR: data sizes differ!\n")
        return ()
    }

    x.ins.name <- "PAPI_TOT_INS"
    x.ev.name <- x$name[1]
    x.benchmark.name <- x$benchmark[1]
    x.ratio.work <- x.work$events / x.work$instructions
    x.ratio.empty <- x.empty$events / x.empty$instructions

    x.last.tenth <- floor(9 * length(x.ratio.work) / 10) : length(x.ratio.work)

    x.ratio.empty.mean <- mean(x.ratio.empty[x.last.tenth])
    x.ratio.work.mean <- mean(x.ratio.work[x.last.tenth])
    x.ratio.ratio <- x.ratio.work.mean / x.ratio.empty.mean

    par(oma=c(0,0,0,0), mar=c(4,4,1,1))
    layout(matrix(c(1,1,2,2,2,2,3,3,4,4,5,5), 2, 6, byrow = TRUE), widths=c(1,1,1,1,1,1), heights=c(1,1))

    plot(c(), c(), ylim=c(0, 1), xlim=c(0, 1), ylab="", xlab="", xaxt="n", yaxt="n", frame.plot=F, main="")
    mtext(side=3, line=-4, text=x.benchmark.name, cex=1.5)
    mtext(side=3, line=-6, text=x.ev.name, cex=1.5)

    legend("center",
        legend=c(
        	"COUNTER / INSTRUCTIONS",
        	sprintf("Empty: %.5f", x.ratio.empty.mean),
        	sprintf("Workload: %.5f", x.ratio.work.mean),
        	"",
        	if (is.na(x.ratio.ratio) || (x.ratio.ratio > 1.)) { "IMPROVEMENT" } else { "WORSENING" },
        	sprintf("%.5f",  x.ratio.ratio)
        ),
        col=c("white", "green", "red","white", "white", "blue"),
        pch=15,
        box.col="white",
        cex=1.5
    )

    my.cmp.plot(x.ratio.work, x.ratio.empty, "Ratio of events to instructions")
    my.plot(x.ratio.work / x.ratio.empty, "Ratio of ratios", col="blue", ylim=my.get.lims(x.ratio.work / x.ratio.empty))
    my.cmp.plot(x.work$events, x.empty$events, "Event count")
    my.cmp.plot(x.work$instructions, x.empty$instructions, "Instruction count")

}

my.stress.plot.all <- function(input, output) {
    x <- read.csv(input)
    my.img.open(output)

    x.benchmarks <- unique(x$benchmark)

    my.printf("Plotting all benchmarks ")

    for (x.b in x.benchmarks) {
        my.stress.plot(subset(x, x$benchmark == x.b))
        my.printf(".")
    }

    my.printf(" done.\n")

    my.img.close()    
}


# Main ...

args <- commandArgs(trailingOnly=T)

options(scipen=7)

if (length(args == 2)) {
    my.stress.plot.all(args[1], args[2])   
}
